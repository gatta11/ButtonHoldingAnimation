using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonHoldingAnimation
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        
            this.Text = "Animated Button Example";
            this.Size = new Size(400, 300);

            // 커스텀 애니메이션 버튼 생성
            AnimatedButton animatedButton = new AnimatedButton(2.0f, 15);
            animatedButton.Size = new Size(200, 100);
            animatedButton.Location = new Point(100, 75);
            animatedButton.Text = "이거슨 버튼";
            animatedButton.Click += AnimatedButton_Click;

            // 폼에 버튼 추가
            this.Controls.Add(animatedButton);
        }

        private void AnimatedButton_Click(object sender, EventArgs e)
        {

            if (((AnimatedButton)sender).IsPressedForDuration())
            {
                MessageBox.Show("2초나 누르고 계셨군요!");
            }

        }
    }
}
