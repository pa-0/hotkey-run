// <copyright file="MainForm.cs" company="PublicDomainWeekly.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>

namespace HotkeyRun
{
    // Directives
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using PublicDomainWeekly;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Gets or sets the associated icon.
        /// </summary>
        /// <value>The associated icon.</value>
        private Icon associatedIcon = null;

        /// <summary>
        /// The settings data.
        /// </summary>
        public SettingsData settingsData = null;

        /// <summary>
        /// The settings data path.
        /// </summary>
        private string settingsDataPath = $"{Application.ProductName}-SettingsData.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HotkeyRun.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();

            // Add keys
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                // Add to list box
                this.keyComboBox.Items.Add(key.ToString());
            }

            /* Set icons */

            // Set associated icon from exe file
            this.associatedIcon = Icon.ExtractAssociatedIcon(typeof(MainForm).GetTypeInfo().Assembly.Location);

            // Set public domain weekly tool strip menu item image
            this.weeklyReleasesPublicDomainWeeklycomToolStripMenuItem.Image = this.associatedIcon.ToBitmap();

            /* Process setiings */

            // Check for settings file
            if (!File.Exists(this.settingsDataPath))
            {
                // Create new settings file
                this.SaveSettingsFile(this.settingsDataPath, new SettingsData());
            }

            // Load settings from disk
            this.settingsData = this.LoadSettingsFile(this.settingsDataPath);

            // Update GUI to reflect settings data
            this.SettingsDataToGui();
        }

        /// <summary>
        /// Settingses the data to GUI.
        /// </summary>
        private void SettingsDataToGui()
        {
            // Topmost
            this.alwaysOnTopToolStripMenuItem.Checked = this.settingsData.TopMost;

            // Modifier checkboxes
            this.controlCheckBox.Checked = this.settingsData.Control;
            this.altCheckBox.Checked = this.settingsData.Alt;
            this.shiftCheckBox.Checked = this.settingsData.Shift;

            // Hotkey
            this.keyComboBox.SelectedItem = this.settingsData.Hotkey;

            // Command list box
            this.programListBox.Items.AddRange(new object[] { this.settingsData.CommandArgumentList });

            // Active/Inactive
            if (this.settingsData.EnableHotkeys)
            {
                // Check active
                this.activeRadioButton.Checked = true;
            }
            else
            {
                // Check inactive
                this.inactiveRadioButton.Checked = true;
            }
        }

        /// <summary>
        /// GUIs to settings sata.
        /// </summary>
        private void GuiToSettingsSata()
        {
            // Topmost
            this.settingsData.TopMost = this.alwaysOnTopToolStripMenuItem.Checked;

            // Modifier checkboxes
            this.settingsData.Control = this.controlCheckBox.Checked;
            this.settingsData.Alt = this.altCheckBox.Checked;
            this.settingsData.Shift = this.shiftCheckBox.Checked;

            // Hotkey
            this.settingsData.Hotkey = this.keyComboBox.SelectedItem.ToString();

            // Command list box
            this.settingsData.CommandArgumentList = this.programListBox.Items.Cast<string>().ToList();

            // Active/Inactive
            this.settingsData.EnableHotkeys = this.activeRadioButton.Checked;
        }

        /// <summary>
        /// Handles the radio button click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRadioButtonClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the browse button click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            // Set properties
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.DefaultExt = "exe";
            this.openFileDialog.Filter = "EXE Files|*.exe|All files (*.*)|*.*";
            this.openFileDialog.Title = "Browse for executable";

            // Show open file dialog
            if (this.openFileDialog.ShowDialog() == DialogResult.OK && this.openFileDialog.FileName.Length > 0)
            {
                // Add to text box
                this.commandTextBox.Text = this.openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Handles the add button click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAddButtonClick(object sender, EventArgs e)
        {
            // Check there's a command
            if (this.commandTextBox.TextLength == 0)
            {
                // Advise user
                MessageBox.Show("Please enter a command to run.", "Empty command", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Focus and halt flow
                goto focusAndExitLabel;
            }

            // Add to list box
            this.programListBox.Items.Add($"{this.commandTextBox.Text}{"\t"}{this.argumentTextBox.Text}");

            // TODO Add to settings data

            // Reset text boxes
            this.commandTextBox.ResetText();
            this.argumentTextBox.ResetText();

        // The focus and exit label
        focusAndExitLabel:

            // Focus command text box
            this.commandTextBox.Focus();
        }

        /// <summary>
        /// Handles the remove button click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the options tool strip menu item drop down item clicked event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOptionsToolStripMenuItemDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Set tool strip menu item
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)e.ClickedItem;

            // Toggle checked
            toolStripMenuItem.Checked = !toolStripMenuItem.Checked;

            // Set topmost by check box
            this.TopMost = this.alwaysOnTopToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the open tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the save tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the exit tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the weekly releases public domain weeklycom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnWeeklyReleasesPublicDomainWeeklycomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the original thread donation codercom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOriginalThreadDonationCodercomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the source code githubcom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the about tool strip menu item click event.
        /// </summary>
        /// <param name="sender">S/// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Loads the settings file.
        /// </summary>
        /// <returns>The settings file.</returns>
        /// <param name="settingsFilePath">Settings file path.</param>
        private SettingsData LoadSettingsFile(string settingsFilePath)
        {
            // Use file stream
            using (FileStream fileStream = File.OpenRead(settingsFilePath))
            {
                // Set xml serialzer
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsData));

                // Return populated settings data
                return xmlSerializer.Deserialize(fileStream) as SettingsData;
            }
        }

        /// <summary>
        /// Saves the settings file.
        /// </summary>
        /// <param name="settingsFilePath">Settings file path.</param>
        /// <param name="settingsDataParam">Settings data parameter.</param>
        private void SaveSettingsFile(string settingsFilePath, SettingsData settingsDataParam)
        {
            try
            {
                // Use stream writer
                using (StreamWriter streamWriter = new StreamWriter(settingsFilePath, false))
                {
                    // Set xml serialzer
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsData));

                    // Serialize settings data
                    xmlSerializer.Serialize(streamWriter, settingsDataParam);
                }
            }
            catch (Exception exception)
            {
                // Advise user
                MessageBox.Show($"Error saving settings file.{Environment.NewLine}{Environment.NewLine}Message:{Environment.NewLine}{exception.Message}", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ons the main form form closing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnMainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO Add code
        }
    }
}
