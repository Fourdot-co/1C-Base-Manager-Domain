using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Threading;
using System.Collections.Generic;

namespace _1C8_Base_Manager
{
    public partial class Form1 : Form
    {
        // ИЗМЕНЕНИЕ: Файл настроек теперь лежит в скрытой системной папке AppData
        private string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "1CBaseManager_Config.txt");
        
        private string currentSourcePath;
        private string currentTargetPath;
        private bool computersLoaded = false;

        public Form1()
        {
            InitializeComponent();
            InitializeLogic();
        }

        private void InitializeLogic()
        {
            SetupGrid(gridTemplates);
            SetupGrid(gridTarget);

            // 1. Загружаем настройки (из скрытой папки)
            LoadSettings();

            // 2. Инициализируем правый список
            comboComputers.Items.Add("localhost");
            comboComputers.SelectedIndex = 0;

            // --- ПРИВЯЗКА СОБЫТИЙ ---
            btnScan.Click += new EventHandler((s, e) => ScanRemoteComputer());
            comboUsers.SelectedIndexChanged += new EventHandler((s, e) => LoadTargetUserBase());
            btnAdd.Click += new EventHandler((s, e) => AddBaseToTarget());
            btnDelete.Click += new EventHandler((s, e) => DeleteFromTarget());
            btnSave.Click += new EventHandler((s, e) => BackupTarget());
            linkGithub.LinkClicked += new LinkLabelLinkClickedEventHandler((s, e) => Process.Start("https://github.com/e-gaydarzhi"));
            
            comboComputers.KeyDown += new KeyEventHandler((s, e) => { if (e.KeyCode == Keys.Enter) ScanRemoteComputer(); });
            comboComputers.DropDown += new EventHandler((s, e) => {
                if (!computersLoaded) { computersLoaded = true; LoadComputersWithTimeout(); }
            });

            btnSelectSource.Click += new EventHandler((s, e) => SelectCustomSourceFile());
            btnDefaultSource.Click += new EventHandler((s, e) => ResetSourceToDefault());
        }

        private void LoadSettings()
        {
            // Путь по умолчанию - это личные базы того, кто запустил программу
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"1C\1CEStart\ibases.v8i");
            
            // Пытаемся прочитать сохраненный путь из скрытого файла
            if (File.Exists(settingsFile))
            {
                try {
                    string savedPath = File.ReadAllText(settingsFile).Trim();
                    // Проверяем, существует ли файл, указанный в настройках
                    if (!string.IsNullOrEmpty(savedPath) && File.Exists(savedPath)) 
                    {
                        defaultPath = savedPath;
                    }
                } catch { }
            }

            SetSourcePath(defaultPath);
        }

        private void SaveSettings(string path)
        {
            try {
                // Записываем путь в скрытый файл в AppData
                File.WriteAllText(settingsFile, path);
            } catch { }
        }

        private void SetSourcePath(string path)
        {
            currentSourcePath = path;
            txtSourcePath.Text = path;
            
            if (!File.Exists(path)) {
                // Если файла вдруг нет (удалили), очищаем таблицу
                gridTemplates.Rows.Clear();
            } else {
                LoadConnectionsFromFile(currentSourcePath, gridTemplates);
            }
        }

        private void SelectCustomSourceFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Список баз 1С (*.v8i)|*.v8i|Все файлы (*.*)|*.*";
            ofd.Title = "Выберите файл со списком баз";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SetSourcePath(ofd.FileName);
                SaveSettings(ofd.FileName); // Сохраняем выбор
            }
        }

        private void ResetSourceToDefault()
        {
            // Сбрасываем на "Мои базы"
            string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"1C\1CEStart\ibases.v8i");
            SetSourcePath(myPath);
            SaveSettings(myPath); 
            statusLabel.Text = "Установлен путь текущего пользователя.";
        }

        // --- ОСТАЛЬНАЯ ЛОГИКА (БЕЗ ИЗМЕНЕНИЙ) ---

        private void LoadComputersWithTimeout()
        {
            statusLabel.Text = "Поиск домена (таймаут 5 сек)...";
            Thread supervisorThread = new Thread(new ThreadStart(delegate {
                List<string> foundComputers = new List<string>();
                Exception threadError = null;
                Thread worker = new Thread(new ThreadStart(delegate {
                    try {
                        DirectoryEntry entry = new DirectoryEntry("LDAP://RootDSE");
                        string domain = (string)entry.Properties["defaultNamingContext"].Value;
                        DirectoryEntry domainEntry = new DirectoryEntry("LDAP://" + domain);
                        DirectorySearcher searcher = new DirectorySearcher(domainEntry);
                        searcher.Filter = "(objectClass=computer)";
                        searcher.SizeLimit = 3000;
                        searcher.PageSize = 100;
                        foreach (SearchResult res in searcher.FindAll()) {
                            string pcName = res.GetDirectoryEntry().Name;
                            if (pcName.StartsWith("CN=")) pcName = pcName.Substring(3);
                            foundComputers.Add(pcName);
                        }
                    } catch (Exception ex) { threadError = ex; }
                }));
                worker.Start();
                if (worker.Join(5000)) {
                    this.Invoke((MethodInvoker)delegate {
                        if (threadError != null) statusLabel.Text = "Ошибка AD: " + threadError.Message + ". Работаем локально.";
                        else {
                            foreach (string pc in foundComputers) if (!comboComputers.Items.Contains(pc)) comboComputers.Items.Add(pc);
                            statusLabel.Text = "Список компьютеров загружен.";
                        }
                    });
                } else {
                    worker.Abort();
                    this.Invoke((MethodInvoker)delegate { statusLabel.Text = "Таймаут: Домен не найден. Работаем локально."; });
                }
            }));
            supervisorThread.IsBackground = true;
            supervisorThread.Start();
        }

        private void SetupGrid(DataGridView grid)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = true;
            grid.RowHeadersVisible = false;
            grid.Columns.Add("Name", "Имя базы");
            grid.Columns.Add("Path", "Путь (Server/File)");
            grid.Columns.Add("ID", "ID");
            grid.Columns["ID"].Visible = false;
        }

        private void ScanRemoteComputer()
        {
            string pcName = comboComputers.Text.Trim();
            if (string.IsNullOrEmpty(pcName)) pcName = "localhost";

            string usersPath = (pcName.ToLower() == "localhost") ? @"C:\Users" : String.Format(@"\\{0}\c$\Users", pcName);

            comboUsers.Items.Clear();
            gridTarget.Rows.Clear();
            statusLabel.Text = "Сканирование " + pcName + "...";
            lblRight.Text = "";

            try {
                if (!Directory.Exists(usersPath)) {
                    MessageBox.Show(String.Format("Не удалось подключиться к {0}.\nПроверьте права админа и доступность ПК.", pcName), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Ошибка доступа.";
                    return;
                }
                string[] dirs = Directory.GetDirectories(usersPath);
                foreach (string d in dirs) comboUsers.Items.Add(new DirectoryInfo(d).Name);
                if (comboUsers.Items.Count > 0) {
                    comboUsers.SelectedIndex = 0;
                    statusLabel.Text = "Найдено пользователей: " + comboUsers.Items.Count;
                }
            } catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void LoadTargetUserBase()
        {
            if (comboUsers.SelectedItem == null) return;
            string pcName = comboComputers.Text.Trim();
            string user = comboUsers.SelectedItem.ToString();
            string basePath = (pcName.ToLower() == "localhost")
               ? String.Format(@"C:\Users\{0}\AppData\Roaming\1C\1CEStart\ibases.v8i", user)
               : String.Format(@"\\{0}\c$\Users\{1}\AppData\Roaming\1C\1CEStart\ibases.v8i", pcName, user);

            currentTargetPath = basePath;
            lblRight.Text = "Базы: " + user;
            LoadConnectionsFromFile(currentTargetPath, gridTarget);
        }

        private void LoadConnectionsFromFile(string path, DataGridView grid)
        {
            grid.Rows.Clear();
            if (!File.Exists(path)) return;
            try {
                string content = File.ReadAllText(path);
                MatchCollection matches = Regex.Matches(content, @"\[([^\]]+)\]([^[]+)(?=\[|$)", RegexOptions.Singleline);
                foreach (Match m in matches) {
                    string name = m.Groups[1].Value.Trim();
                    string body = m.Groups[2].Value;
                    string connect = Extract(body, @"Connect=(.*?)(?:\r|\n|$)");
                    string id = Extract(body, @"ID=([^\r\n]+)");
                    string displayPath = connect.Replace("Srvr=", "SRV: ").Replace("File=", "FILE: ").Replace(";", " ").Replace("Ref=", "/").Replace("\"", "");
                    grid.Rows.Add(name, displayPath, id, body);
                }
            } catch (Exception ex) { statusLabel.Text = "Ошибка чтения: " + ex.Message; }
        }

        private string Extract(string text, string pattern)
        {
            Match m = Regex.Match(text, pattern);
            return m.Success ? m.Groups[1].Value.Trim() : "";
        }

        private void AddBaseToTarget()
        {
            if (gridTemplates.SelectedRows.Count == 0) return;
            if (string.IsNullOrEmpty(currentTargetPath)) { MessageBox.Show("Выберите пользователя справа!"); return; }

            try {
                string dir = Path.GetDirectoryName(currentTargetPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                using (StreamWriter sw = File.AppendText(currentTargetPath)) {
                    foreach (DataGridViewRow row in gridTemplates.SelectedRows) {
                        string name = row.Cells[0].Value.ToString();
                        string rawConnect = row.Cells[1].Value.ToString();
                        string newId = Guid.NewGuid().ToString();
                        string connectionString = rawConnect;
                        
                        if(rawConnect.StartsWith("SRV:")) {
                            var parts = rawConnect.Replace("SRV:", "").Split('/');
                            if(parts.Length >= 2) {
                                string srv = parts[0].Trim().Replace("\"", "");
                                string refBase = parts[1].Trim().Replace("\"", "");
                                connectionString = String.Format("Srvr=\"{0}\";Ref=\"{1}\";", srv, refBase);
                            }
                        } else if (rawConnect.StartsWith("FILE:")) {
                             string filePath = rawConnect.Replace("FILE:", "").Trim().Replace("\"", "");
                             connectionString = String.Format("File=\"{0}\";", filePath);
                        }
                        sw.WriteLine(String.Format("\r\n[{0}]\r\nConnect={1}\r\nID={2}\r\nOrderInList=16384\r\nFolder=/\r\nOrderInTree=256\r\nExternal=0\r\nClientConnectionSpeed=Normal\r\nApp=Auto\r\nWA=1\r\nVersion=8.3", name, connectionString, newId));
                    }
                }
                statusLabel.Text = "Базы добавлены!";
                LoadConnectionsFromFile(currentTargetPath, gridTarget);
            } catch (Exception ex) { MessageBox.Show("Ошибка записи: " + ex.Message); }
        }

        private void DeleteFromTarget()
        {
            if (gridTarget.SelectedRows.Count == 0) return;
            if (!File.Exists(currentTargetPath)) return;
            if (MessageBox.Show("Удалить выбранные базы?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try {
                string content = File.ReadAllText(currentTargetPath);
                foreach (DataGridViewRow row in gridTarget.SelectedRows) {
                    string name = row.Cells[0].Value.ToString();
                    string pattern = String.Format(@"\[{0}\][^\[]*(?=\[|$)", Regex.Escape(name));
                    content = Regex.Replace(content, pattern, "");
                }
                content = Regex.Replace(content, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                File.WriteAllText(currentTargetPath, content);
                LoadConnectionsFromFile(currentTargetPath, gridTarget);
                statusLabel.Text = "Удалено.";
            } catch (Exception ex) { MessageBox.Show("Ошибка удаления: " + ex.Message); }
        }

        private void BackupTarget()
        {
            if (!File.Exists(currentTargetPath)) return;
            try {
                File.Copy(currentTargetPath, currentTargetPath + ".bak_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                MessageBox.Show("Резервная копия создана!");
            } catch(Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }
    }
}