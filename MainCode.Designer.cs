namespace cureach
{
    partial class MainCode
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Canvas = new PictureBox();
            Modes = new Label();
            DrawLine = new RadioButton();
            DrawCircle = new RadioButton();
            DrawArc = new RadioButton();
            DrawPolyline = new RadioButton();
            TextSettings = new Label();
            AddText = new RadioButton();
            fontDialog1 = new FontDialog();
            ChooseFont = new ComboBox();
            ChooseFontSize = new ComboBox();
            FontName = new Label();
            FontSize = new Label();
            FontStyle = new Label();
            ChooseFontStyle = new ComboBox();
            RLText = new RadioButton();
            Delete = new RadioButton();
            ClearCanvas = new Button();
            SaveInFile = new Button();
            DeleteNotFullRect = new RadioButton();
            DeleteFullRect = new RadioButton();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            OpenFromFile = new Button();
            SaveList = new Button();
            OpenList = new Button();
            DeleteSettings = new Label();
            ((System.ComponentModel.ISupportInitialize)Canvas).BeginInit();
            SuspendLayout();
            // 
            // Canvas
            // 
            Canvas.Location = new Point(12, 43);
            Canvas.Name = "Canvas";
            Canvas.Size = new Size(863, 619);
            Canvas.TabIndex = 0;
            Canvas.TabStop = false;
            Canvas.Paint += Canvas_Paint;
            Canvas.MouseClick += Canvas_MouseClick;
            Canvas.MouseDoubleClick += Canvas_MouseDoubleClick;
            Canvas.MouseMove += Canvas_MouseMove;
            // 
            // Modes
            // 
            Modes.AutoSize = true;
            Modes.Location = new Point(881, 43);
            Modes.Name = "Modes";
            Modes.Size = new Size(70, 20);
            Modes.TabIndex = 1;
            Modes.Text = "Режимы:";
            // 
            // DrawLine
            // 
            DrawLine.AutoSize = true;
            DrawLine.Location = new Point(885, 70);
            DrawLine.Name = "DrawLine";
            DrawLine.Size = new Size(87, 24);
            DrawLine.TabIndex = 2;
            DrawLine.TabStop = true;
            DrawLine.Text = "Отрезок";
            DrawLine.UseVisualStyleBackColor = true;
            // 
            // DrawCircle
            // 
            DrawCircle.AutoSize = true;
            DrawCircle.Location = new Point(885, 100);
            DrawCircle.Name = "DrawCircle";
            DrawCircle.Size = new Size(114, 24);
            DrawCircle.TabIndex = 3;
            DrawCircle.TabStop = true;
            DrawCircle.Text = "Окружность";
            DrawCircle.UseVisualStyleBackColor = true;
            // 
            // DrawArc
            // 
            DrawArc.AutoSize = true;
            DrawArc.Location = new Point(885, 130);
            DrawArc.Name = "DrawArc";
            DrawArc.Size = new Size(61, 24);
            DrawArc.TabIndex = 5;
            DrawArc.TabStop = true;
            DrawArc.Text = "Дуга";
            DrawArc.UseVisualStyleBackColor = true;
            // 
            // DrawPolyline
            // 
            DrawPolyline.AutoSize = true;
            DrawPolyline.Location = new Point(885, 160);
            DrawPolyline.Name = "DrawPolyline";
            DrawPolyline.Size = new Size(110, 24);
            DrawPolyline.TabIndex = 7;
            DrawPolyline.TabStop = true;
            DrawPolyline.Text = "Полилиния";
            DrawPolyline.UseVisualStyleBackColor = true;
            // 
            // TextSettings
            // 
            TextSettings.AutoSize = true;
            TextSettings.Location = new Point(881, 198);
            TextSettings.Name = "TextSettings";
            TextSettings.Size = new Size(133, 20);
            TextSettings.TabIndex = 8;
            TextSettings.Text = "Настройки текста:";
            // 
            // AddText
            // 
            AddText.AutoSize = true;
            AddText.Location = new Point(887, 230);
            AddText.Name = "AddText";
            AddText.Size = new Size(64, 24);
            AddText.TabIndex = 9;
            AddText.TabStop = true;
            AddText.Text = "Ввод";
            AddText.UseVisualStyleBackColor = true;
            // 
            // ChooseFont
            // 
            ChooseFont.FormattingEnabled = true;
            ChooseFont.Location = new Point(884, 319);
            ChooseFont.Name = "ChooseFont";
            ChooseFont.Size = new Size(142, 28);
            ChooseFont.TabIndex = 11;
            ChooseFont.SelectedIndexChanged += ChooseFont_SelectedIndexChanged;
            // 
            // ChooseFontSize
            // 
            ChooseFontSize.FormattingEnabled = true;
            ChooseFontSize.Items.AddRange(new object[] { "8", "9", "10", "11", "12", "14", "16", "18", "24", "30", "36", "48", "60", "72" });
            ChooseFontSize.Location = new Point(884, 373);
            ChooseFontSize.Name = "ChooseFontSize";
            ChooseFontSize.Size = new Size(142, 28);
            ChooseFontSize.TabIndex = 12;
            ChooseFontSize.SelectedIndexChanged += ChooseFontSize_SelectedIndexChanged;
            // 
            // FontName
            // 
            FontName.AutoSize = true;
            FontName.Location = new Point(881, 296);
            FontName.Name = "FontName";
            FontName.Size = new Size(60, 20);
            FontName.TabIndex = 13;
            FontName.Text = "Шрифт:";
            // 
            // FontSize
            // 
            FontSize.AutoSize = true;
            FontSize.Location = new Point(881, 350);
            FontSize.Name = "FontSize";
            FontSize.Size = new Size(121, 20);
            FontSize.TabIndex = 14;
            FontSize.Text = "Размер шрифта:";
            // 
            // FontStyle
            // 
            FontStyle.AutoSize = true;
            FontStyle.Location = new Point(881, 404);
            FontStyle.Name = "FontStyle";
            FontStyle.Size = new Size(96, 20);
            FontStyle.TabIndex = 16;
            FontStyle.Text = "Тип шрифта:";
            // 
            // ChooseFontStyle
            // 
            ChooseFontStyle.FormattingEnabled = true;
            ChooseFontStyle.Location = new Point(884, 427);
            ChooseFontStyle.Name = "ChooseFontStyle";
            ChooseFontStyle.Size = new Size(142, 28);
            ChooseFontStyle.TabIndex = 15;
            ChooseFontStyle.SelectedIndexChanged += ChooseFontStyle_SelectedIndexChanged;
            // 
            // RLText
            // 
            RLText.AutoSize = true;
            RLText.Location = new Point(887, 260);
            RLText.Name = "RLText";
            RLText.Size = new Size(132, 24);
            RLText.TabIndex = 17;
            RLText.TabStop = true;
            RLText.Text = "Редактировать";
            RLText.UseVisualStyleBackColor = true;
            // 
            // Delete
            // 
            Delete.AutoSize = true;
            Delete.Location = new Point(887, 503);
            Delete.Name = "Delete";
            Delete.Size = new Size(152, 24);
            Delete.TabIndex = 18;
            Delete.TabStop = true;
            Delete.Text = "Удаление кликом";
            Delete.UseVisualStyleBackColor = true;
            // 
            // ClearCanvas
            // 
            ClearCanvas.Location = new Point(600, 8);
            ClearCanvas.Name = "ClearCanvas";
            ClearCanvas.Size = new Size(142, 29);
            ClearCanvas.TabIndex = 19;
            ClearCanvas.Text = "Очистить";
            ClearCanvas.UseVisualStyleBackColor = true;
            ClearCanvas.Click += ClearCanvas_Click;
            // 
            // SaveInFile
            // 
            SaveInFile.Location = new Point(12, 8);
            SaveInFile.Name = "SaveInFile";
            SaveInFile.Size = new Size(141, 29);
            SaveInFile.TabIndex = 20;
            SaveInFile.Text = "Сохранить в .jpg";
            SaveInFile.UseVisualStyleBackColor = true;
            SaveInFile.Click += SaveInFile_Click;
            // 
            // DeleteNotFullRect
            // 
            DeleteNotFullRect.AutoSize = true;
            DeleteNotFullRect.Location = new Point(887, 533);
            DeleteNotFullRect.Name = "DeleteNotFullRect";
            DeleteNotFullRect.Size = new Size(153, 24);
            DeleteNotFullRect.TabIndex = 21;
            DeleteNotFullRect.TabStop = true;
            DeleteNotFullRect.Text = "Прямоугольник 1";
            DeleteNotFullRect.UseVisualStyleBackColor = true;
            // 
            // DeleteFullRect
            // 
            DeleteFullRect.AutoSize = true;
            DeleteFullRect.Location = new Point(887, 563);
            DeleteFullRect.Name = "DeleteFullRect";
            DeleteFullRect.Size = new Size(153, 24);
            DeleteFullRect.TabIndex = 22;
            DeleteFullRect.TabStop = true;
            DeleteFullRect.Text = "Прямоугольник 2";
            DeleteFullRect.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // OpenFromFile
            // 
            OpenFromFile.Location = new Point(159, 8);
            OpenFromFile.Name = "OpenFromFile";
            OpenFromFile.Size = new Size(141, 29);
            OpenFromFile.TabIndex = 23;
            OpenFromFile.Text = "Открыть .jpg";
            OpenFromFile.UseVisualStyleBackColor = true;
            OpenFromFile.Click += OpenFromFile_Click;
            // 
            // SaveList
            // 
            SaveList.Location = new Point(306, 8);
            SaveList.Name = "SaveList";
            SaveList.Size = new Size(141, 29);
            SaveList.TabIndex = 24;
            SaveList.Text = "Сохранить obj";
            SaveList.UseVisualStyleBackColor = true;
            SaveList.Click += SaveList_Click;
            // 
            // OpenList
            // 
            OpenList.Location = new Point(453, 8);
            OpenList.Name = "OpenList";
            OpenList.Size = new Size(141, 29);
            OpenList.TabIndex = 25;
            OpenList.Text = "Открыть obj";
            OpenList.UseVisualStyleBackColor = true;
            OpenList.Click += OpenList_Click;
            // 
            // DeleteSettings
            // 
            DeleteSettings.AutoSize = true;
            DeleteSettings.Location = new Point(881, 471);
            DeleteSettings.Name = "DeleteSettings";
            DeleteSettings.Size = new Size(79, 20);
            DeleteSettings.TabIndex = 26;
            DeleteSettings.Text = "Удаление:";
            // 
            // MainCode
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 676);
            Controls.Add(DeleteSettings);
            Controls.Add(OpenList);
            Controls.Add(SaveList);
            Controls.Add(OpenFromFile);
            Controls.Add(DeleteFullRect);
            Controls.Add(DeleteNotFullRect);
            Controls.Add(SaveInFile);
            Controls.Add(ClearCanvas);
            Controls.Add(Delete);
            Controls.Add(RLText);
            Controls.Add(ChooseFont);
            Controls.Add(ChooseFontSize);
            Controls.Add(FontStyle);
            Controls.Add(FontName);
            Controls.Add(ChooseFontStyle);
            Controls.Add(AddText);
            Controls.Add(FontSize);
            Controls.Add(TextSettings);
            Controls.Add(DrawPolyline);
            Controls.Add(DrawArc);
            Controls.Add(DrawCircle);
            Controls.Add(DrawLine);
            Controls.Add(Modes);
            Controls.Add(Canvas);
            Name = "MainCode";
            Text = "Курсовая В№18";
            Load += MainCode_Load;
            ((System.ComponentModel.ISupportInitialize)Canvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox Canvas;
        private Label Modes;
        private RadioButton DrawLine;
        private RadioButton DrawCircle;
        private RadioButton DrawArc;
        private RadioButton DrawPolyline;
        private Label TextSettings;
        private RadioButton AddText;
        private FontDialog fontDialog1;
        private ComboBox ChooseFont;
        private ComboBox ChooseFontSize;
        private Label FontName;
        private Label FontSize;
        private Label FontStyle;
        private ComboBox ChooseFontStyle;
        private RadioButton RLText;
        private RadioButton Delete;
        private Button ClearCanvas;
        private Button SaveInFile;
        private RadioButton DeleteNotFullRect;
        private RadioButton DeleteFullRect;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private Button OpenFromFile;
        private Button SaveList;
        private Button OpenList;
        private Label DeleteSettings;
    }
}