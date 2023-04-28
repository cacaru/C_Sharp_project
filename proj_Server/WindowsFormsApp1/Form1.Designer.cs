namespace Server
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.hand0 = new System.Windows.Forms.Button();
            this.hand1 = new System.Windows.Forms.Button();
            this.hand2 = new System.Windows.Forms.Button();
            this.hand3 = new System.Windows.Forms.Button();
            this.hand4 = new System.Windows.Forms.Button();
            this.btnend = new System.Windows.Forms.Button();
            this.seosun = new System.Windows.Forms.TextBox();
            this.field = new System.Windows.Forms.TextBox();
            this.stat1 = new System.Windows.Forms.TextBox();
            this.stat2 = new System.Windows.Forms.TextBox();
            this.stat4 = new System.Windows.Forms.TextBox();
            this.stat3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // hand0
            // 
            this.hand0.Location = new System.Drawing.Point(1, 332);
            this.hand0.Name = "hand0";
            this.hand0.Size = new System.Drawing.Size(75, 56);
            this.hand0.TabIndex = 0;
            this.hand0.UseVisualStyleBackColor = true;
            this.hand0.Click += new System.EventHandler(this.hand0_Click);
            // 
            // hand1
            // 
            this.hand1.Location = new System.Drawing.Point(100, 332);
            this.hand1.Name = "hand1";
            this.hand1.Size = new System.Drawing.Size(75, 56);
            this.hand1.TabIndex = 1;
            this.hand1.UseVisualStyleBackColor = true;
            this.hand1.Click += new System.EventHandler(this.hand1_Click);
            // 
            // hand2
            // 
            this.hand2.Location = new System.Drawing.Point(195, 332);
            this.hand2.Name = "hand2";
            this.hand2.Size = new System.Drawing.Size(75, 56);
            this.hand2.TabIndex = 2;
            this.hand2.UseVisualStyleBackColor = true;
            this.hand2.Click += new System.EventHandler(this.hand2_Click);
            // 
            // hand3
            // 
            this.hand3.Location = new System.Drawing.Point(285, 332);
            this.hand3.Name = "hand3";
            this.hand3.Size = new System.Drawing.Size(75, 56);
            this.hand3.TabIndex = 3;
            this.hand3.UseVisualStyleBackColor = true;
            this.hand3.Click += new System.EventHandler(this.hand3_Click);
            // 
            // hand4
            // 
            this.hand4.Location = new System.Drawing.Point(377, 332);
            this.hand4.Name = "hand4";
            this.hand4.Size = new System.Drawing.Size(75, 56);
            this.hand4.TabIndex = 4;
            this.hand4.UseVisualStyleBackColor = true;
            this.hand4.Click += new System.EventHandler(this.hand4_Click);
            // 
            // btnend
            // 
            this.btnend.Location = new System.Drawing.Point(549, 314);
            this.btnend.Name = "btnend";
            this.btnend.Size = new System.Drawing.Size(75, 23);
            this.btnend.TabIndex = 5;
            this.btnend.Text = "턴 종료";
            this.btnend.UseVisualStyleBackColor = true;
            this.btnend.Click += new System.EventHandler(this.btnend_Click);
            // 
            // seosun
            // 
            this.seosun.Location = new System.Drawing.Point(28, 259);
            this.seosun.Multiline = true;
            this.seosun.Name = "seosun";
            this.seosun.Size = new System.Drawing.Size(242, 54);
            this.seosun.TabIndex = 6;
            // 
            // field
            // 
            this.field.Location = new System.Drawing.Point(28, 66);
            this.field.Multiline = true;
            this.field.Name = "field";
            this.field.Size = new System.Drawing.Size(353, 170);
            this.field.TabIndex = 7;
            // 
            // stat1
            // 
            this.stat1.Location = new System.Drawing.Point(392, -2);
            this.stat1.Multiline = true;
            this.stat1.Name = "stat1";
            this.stat1.Size = new System.Drawing.Size(100, 147);
            this.stat1.TabIndex = 8;
            // 
            // stat2
            // 
            this.stat2.Location = new System.Drawing.Point(498, -2);
            this.stat2.Multiline = true;
            this.stat2.Name = "stat2";
            this.stat2.Size = new System.Drawing.Size(100, 147);
            this.stat2.TabIndex = 9;
            // 
            // stat4
            // 
            this.stat4.Location = new System.Drawing.Point(498, 151);
            this.stat4.Multiline = true;
            this.stat4.Name = "stat4";
            this.stat4.Size = new System.Drawing.Size(100, 129);
            this.stat4.TabIndex = 10;
            // 
            // stat3
            // 
            this.stat3.Location = new System.Drawing.Point(392, 151);
            this.stat3.Multiline = true;
            this.stat3.Name = "stat3";
            this.stat3.Size = new System.Drawing.Size(100, 129);
            this.stat3.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 413);
            this.Controls.Add(this.stat3);
            this.Controls.Add(this.stat4);
            this.Controls.Add(this.stat2);
            this.Controls.Add(this.stat1);
            this.Controls.Add(this.field);
            this.Controls.Add(this.seosun);
            this.Controls.Add(this.btnend);
            this.Controls.Add(this.hand4);
            this.Controls.Add(this.hand3);
            this.Controls.Add(this.hand2);
            this.Controls.Add(this.hand1);
            this.Controls.Add(this.hand0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button hand0;
        private System.Windows.Forms.Button hand1;
        private System.Windows.Forms.Button hand2;
        private System.Windows.Forms.Button hand3;
        private System.Windows.Forms.Button hand4;
        private System.Windows.Forms.Button btnend;
        private System.Windows.Forms.TextBox seosun;
        private System.Windows.Forms.TextBox field;
        private System.Windows.Forms.TextBox stat1;
        private System.Windows.Forms.TextBox stat2;
        private System.Windows.Forms.TextBox stat4;
        private System.Windows.Forms.TextBox stat3;
    }
}

