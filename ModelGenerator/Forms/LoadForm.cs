﻿using System;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();

            CancelLink.Visible = false;
        }


        public void SetTime(string sec)
        {
            skinLabel2.Text = $"{sec}秒";
        }

        static LoadForm _loadForm = null;

        static Form _parent;

        public static void ShowLoading(Form parent)
        {
            _parent = parent;

            _parent.Invoke(new Action(() =>
            {
                if (_loadForm == null)
                {
                    _loadForm = new LoadForm();
                }
            }));

            _loadForm.ShowDialogWithLoopAsync(_parent, (seconds) =>
            {
                if (int.TryParse(seconds, out int os) && (os > 10 && !_loadForm.CancelLink.Visible))
                {
                    _loadForm.CancelLink.Visible = true;
                }
                _loadForm.SetTime(seconds);
            });
        }

        public static void HideLoading(Form parent = null)
        {
            _loadForm.HideDialogAsync(parent ?? _parent);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HideLoading();
        }
    }
}
