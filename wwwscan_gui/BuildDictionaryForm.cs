﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace wwwscan_gui
{
    public partial class BuildDictionaryForm : Form
    {

        private readonly string _base_dicpath = Application.StartupPath + "\\dic\\";

        public BuildDictionaryForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        private void ControlsDisable(bool turnon)
        {
            foreach (Control item in this.Controls)
            {
                if (!item.Equals(this.textBox_result))
                {
                    item.Enabled = turnon;
                }

            }

        }



        private void button_openfile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_filepath.Text = openFileDialog1.FileName;
            }
        }

        private void backgroundWorker_import_DoWork(object sender, DoWorkEventArgs e)
        {

            if (!Directory.Exists(_base_dicpath))
            {
                Directory.CreateDirectory(_base_dicpath);
            }

            var factory = new TXTFactory();
            var list = factory.ReadTXT(this.textBox_filepath.Text);
            var listlist = factory.PrepareDics(list);

            List<string> index = new List<string>(listlist.Keys);
            for (int i = 0; i < listlist.Count; i++)
            {
                factory.WriteTXT(Path.Combine(_base_dicpath, index[i]), listlist[index[i]], false);
                backgroundWorker_import.ReportProgress(100 * i / listlist.Count);
            }
        }

        private void backgroundWorker_import_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_import_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Value = 100;
            this.textBox_result.Text += "导入成功！\r\n";
            //this.textBox_result.Text += string.Format("共导入aspx{0}条\r\nbackup{1}条\r\ndir{2}条\r\neditor{3}条\r\nhtml{4}条\r\njava{5}条\r\nother{6}条\r\nphp{7}条\r\n导入完成！\r\n",
            //                                        num_aspx,
            //                                        num_backup,
            //                                        num_dir,
            //                                        num_editor,
            //                                        num_html,
            //                                        num_java,
            //                                        num_other,
            //                                        num_php);
            ControlsDisable(true);
        }

        private void backgroundWorker_distinct_DoWork(object sender, DoWorkEventArgs e)
        {
            var factory = new TXTFactory();
            var files = Directory.GetFiles(_base_dicpath);

            for (int i = 0; i < files.Length; i++)
            {
                factory.DistinctTXT(files[i]);
                this.backgroundWorker_distinct.ReportProgress(100 * i / files.Length);
            }
        }

        private void backgroundWorker_distinct_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_distinct_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Value = 100;
            this.textBox_result.Text += "清理完成！\r\n";
            ControlsDisable(true);
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.textBox_filepath.Text))
            {
                MessageBox.Show("文件名有误或不存在");
                this.textBox_filepath.Focus();
            }
            else
            {
                ControlsDisable(false);
                this.progressBar1.Value = 0;
                this.textBox_result.Text += "开始导入！\r\n";

                backgroundWorker_import.RunWorkerAsync();
            }
        }



        private void textBox_result_TextChanged(object sender, EventArgs e)
        {
            this.textBox_result.SelectionStart = this.textBox_result.Text.Length;
            this.textBox_result.ScrollToCaret();
        }

        private void button_distinct_Click(object sender, EventArgs e)
        {
            ControlsDisable(false);
            this.progressBar1.Value = 0;
            this.textBox_result.Text += "开始清理！\r\n";
            this.backgroundWorker_distinct.RunWorkerAsync();
        }

        private void BuildDictionaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker_distinct.IsBusy || this.backgroundWorker_import.IsBusy)
            {
                var result = MessageBox.Show("强行关闭会造成字典缺失，确认继续？", "任务进行中！", MessageBoxButtons.OKCancel);
                e.Cancel = result != DialogResult.OK;
            }
        }

    }
}
