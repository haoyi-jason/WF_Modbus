namespace WF_Modbus
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRemoveInst = new System.Windows.Forms.Button();
            this.btnAddInst = new System.Windows.Forms.Button();
            this.btnInstLoad = new System.Windows.Forms.Button();
            this.btnInstSave = new System.Windows.Forms.Button();
            this.btnInstWrite = new System.Windows.Forms.Button();
            this.btnInstRead = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSaveRecipe = new System.Windows.Forms.Button();
            this.btnLoadRecipe = new System.Windows.Forms.Button();
            this.btnRemoveProc = new System.Windows.Forms.Button();
            this.btnAddProc = new System.Windows.Forms.Button();
            this.btnExeRecipe = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lbRecipe = new System.Windows.Forms.ListBox();
            this.btnInsertProc = new System.Windows.Forms.Button();
            this.btnInsertInstrument = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtFlashMessage = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDUTSerial = new System.Windows.Forms.TextBox();
            this.btnTestStep1 = new System.Windows.Forms.Button();
            this.btnTestStep2 = new System.Windows.Forms.Button();
            this.btnTestStep3 = new System.Windows.Forms.Button();
            this.btnTestStep4 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbWorkingDir = new System.Windows.Forms.TextBox();
            this.btnWorkingDir = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStopExec = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbRecipeIndex = new System.Windows.Forms.TextBox();
            this.btnSaveBenchAs = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnAddRecipe = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnImageFlash = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.tbImagePath = new System.Windows.Forms.TextBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.btnExecRecipe = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(182, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1101, 423);
            this.dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(217, 6);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1066, 423);
            this.dataGridView2.TabIndex = 2;
            this.dataGridView2.CurrentCellChanged += new System.EventHandler(this.dataGridView2_CurrentCellChanged);
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView2_DataBindingComplete);
            this.dataGridView2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1297, 461);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1289, 435);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Instrument Definitions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRemoveInst
            // 
            this.btnRemoveInst.Location = new System.Drawing.Point(6, 154);
            this.btnRemoveInst.Name = "btnRemoveInst";
            this.btnRemoveInst.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveInst.TabIndex = 7;
            this.btnRemoveInst.Text = "Remove";
            this.btnRemoveInst.UseVisualStyleBackColor = true;
            this.btnRemoveInst.Click += new System.EventHandler(this.btnRemoveInst_Click);
            // 
            // btnAddInst
            // 
            this.btnAddInst.Location = new System.Drawing.Point(6, 79);
            this.btnAddInst.Name = "btnAddInst";
            this.btnAddInst.Size = new System.Drawing.Size(75, 23);
            this.btnAddInst.TabIndex = 6;
            this.btnAddInst.Text = "Add";
            this.btnAddInst.UseVisualStyleBackColor = true;
            this.btnAddInst.Click += new System.EventHandler(this.btnAddInst_Click);
            // 
            // btnInstLoad
            // 
            this.btnInstLoad.Location = new System.Drawing.Point(9, 21);
            this.btnInstLoad.Name = "btnInstLoad";
            this.btnInstLoad.Size = new System.Drawing.Size(75, 23);
            this.btnInstLoad.TabIndex = 5;
            this.btnInstLoad.Text = "Load";
            this.btnInstLoad.UseVisualStyleBackColor = true;
            this.btnInstLoad.Click += new System.EventHandler(this.btnInstLoad_Click);
            // 
            // btnInstSave
            // 
            this.btnInstSave.Location = new System.Drawing.Point(9, 50);
            this.btnInstSave.Name = "btnInstSave";
            this.btnInstSave.Size = new System.Drawing.Size(75, 23);
            this.btnInstSave.TabIndex = 4;
            this.btnInstSave.Text = "Save";
            this.btnInstSave.UseVisualStyleBackColor = true;
            this.btnInstSave.Click += new System.EventHandler(this.btnInstSave_Click);
            // 
            // btnInstWrite
            // 
            this.btnInstWrite.Location = new System.Drawing.Point(6, 50);
            this.btnInstWrite.Name = "btnInstWrite";
            this.btnInstWrite.Size = new System.Drawing.Size(75, 23);
            this.btnInstWrite.TabIndex = 3;
            this.btnInstWrite.Text = "Write";
            this.btnInstWrite.UseVisualStyleBackColor = true;
            this.btnInstWrite.Click += new System.EventHandler(this.btnInstWrite_Click);
            // 
            // btnInstRead
            // 
            this.btnInstRead.Location = new System.Drawing.Point(6, 21);
            this.btnInstRead.Name = "btnInstRead";
            this.btnInstRead.Size = new System.Drawing.Size(75, 23);
            this.btnInstRead.TabIndex = 2;
            this.btnInstRead.Text = "Read";
            this.btnInstRead.UseVisualStyleBackColor = true;
            this.btnInstRead.Click += new System.EventHandler(this.btnInstRead_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1289, 435);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Recipe Definitions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSaveRecipe
            // 
            this.btnSaveRecipe.Location = new System.Drawing.Point(66, 21);
            this.btnSaveRecipe.Name = "btnSaveRecipe";
            this.btnSaveRecipe.Size = new System.Drawing.Size(57, 23);
            this.btnSaveRecipe.TabIndex = 7;
            this.btnSaveRecipe.Text = "Save";
            this.btnSaveRecipe.UseVisualStyleBackColor = true;
            this.btnSaveRecipe.Click += new System.EventHandler(this.btnSaveRecipe_Click);
            // 
            // btnLoadRecipe
            // 
            this.btnLoadRecipe.Location = new System.Drawing.Point(6, 21);
            this.btnLoadRecipe.Name = "btnLoadRecipe";
            this.btnLoadRecipe.Size = new System.Drawing.Size(54, 23);
            this.btnLoadRecipe.TabIndex = 6;
            this.btnLoadRecipe.Text = "Load";
            this.btnLoadRecipe.UseVisualStyleBackColor = true;
            this.btnLoadRecipe.Click += new System.EventHandler(this.btnLoadRecipe_Click);
            // 
            // btnRemoveProc
            // 
            this.btnRemoveProc.Location = new System.Drawing.Point(6, 108);
            this.btnRemoveProc.Name = "btnRemoveProc";
            this.btnRemoveProc.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveProc.TabIndex = 5;
            this.btnRemoveProc.Text = "Remove";
            this.btnRemoveProc.UseVisualStyleBackColor = true;
            this.btnRemoveProc.Click += new System.EventHandler(this.btnRemoveRecipe_Click);
            // 
            // btnAddProc
            // 
            this.btnAddProc.Location = new System.Drawing.Point(6, 50);
            this.btnAddProc.Name = "btnAddProc";
            this.btnAddProc.Size = new System.Drawing.Size(75, 23);
            this.btnAddProc.TabIndex = 4;
            this.btnAddProc.Text = "Add";
            this.btnAddProc.UseVisualStyleBackColor = true;
            this.btnAddProc.Click += new System.EventHandler(this.btnAddRecipe_Click);
            // 
            // btnExeRecipe
            // 
            this.btnExeRecipe.Location = new System.Drawing.Point(6, 21);
            this.btnExeRecipe.Name = "btnExeRecipe";
            this.btnExeRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnExeRecipe.TabIndex = 3;
            this.btnExeRecipe.Text = "Execute";
            this.btnExeRecipe.UseVisualStyleBackColor = true;
            this.btnExeRecipe.Click += new System.EventHandler(this.btnExeRecipe_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1289, 435);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DUT測試";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lbRecipe
            // 
            this.lbRecipe.FormattingEnabled = true;
            this.lbRecipe.ItemHeight = 12;
            this.lbRecipe.Location = new System.Drawing.Point(3, 57);
            this.lbRecipe.Name = "lbRecipe";
            this.lbRecipe.Size = new System.Drawing.Size(191, 76);
            this.lbRecipe.TabIndex = 8;
            this.lbRecipe.SelectedIndexChanged += new System.EventHandler(this.lbRecipe_SelectedIndexChanged);
            this.lbRecipe.DoubleClick += new System.EventHandler(this.lbRecipe_DoubleClick);
            // 
            // btnInsertProc
            // 
            this.btnInsertProc.Location = new System.Drawing.Point(6, 79);
            this.btnInsertProc.Name = "btnInsertProc";
            this.btnInsertProc.Size = new System.Drawing.Size(75, 23);
            this.btnInsertProc.TabIndex = 9;
            this.btnInsertProc.Text = "Insert";
            this.btnInsertProc.UseVisualStyleBackColor = true;
            this.btnInsertProc.Click += new System.EventHandler(this.btnInsertRecipe_Click);
            // 
            // btnInsertInstrument
            // 
            this.btnInsertInstrument.Location = new System.Drawing.Point(6, 108);
            this.btnInsertInstrument.Name = "btnInsertInstrument";
            this.btnInsertInstrument.Size = new System.Drawing.Size(75, 23);
            this.btnInsertInstrument.TabIndex = 8;
            this.btnInsertInstrument.Text = "Insert";
            this.btnInsertInstrument.UseVisualStyleBackColor = true;
            this.btnInsertInstrument.Click += new System.EventHandler(this.btnInsertInstrument_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnWorkingDir);
            this.groupBox1.Controls.Add(this.tbWorkingDir);
            this.groupBox1.Controls.Add(this.rtFlashMessage);
            this.groupBox1.Location = new System.Drawing.Point(6, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1277, 66);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "步驟-1 : 設定存檔路徑";
            // 
            // rtFlashMessage
            // 
            this.rtFlashMessage.Location = new System.Drawing.Point(569, 10);
            this.rtFlashMessage.Name = "rtFlashMessage";
            this.rtFlashMessage.Size = new System.Drawing.Size(696, 50);
            this.rtFlashMessage.TabIndex = 3;
            this.rtFlashMessage.Text = "燒錄後須留意板上的綠色LED灯應該在閃爍!";
            this.rtFlashMessage.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnTestStep4);
            this.groupBox2.Controls.Add(this.btnTestStep3);
            this.groupBox2.Controls.Add(this.btnTestStep2);
            this.groupBox2.Controls.Add(this.btnTestStep1);
            this.groupBox2.Controls.Add(this.tbDUTSerial);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1277, 99);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "步驟-2 輸入序號及測試";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "序號";
            // 
            // tbDUTSerial
            // 
            this.tbDUTSerial.Location = new System.Drawing.Point(74, 20);
            this.tbDUTSerial.Name = "tbDUTSerial";
            this.tbDUTSerial.Size = new System.Drawing.Size(335, 22);
            this.tbDUTSerial.TabIndex = 1;
            this.tbDUTSerial.Text = "12345";
            // 
            // btnTestStep1
            // 
            this.btnTestStep1.Location = new System.Drawing.Point(449, 21);
            this.btnTestStep1.Name = "btnTestStep1";
            this.btnTestStep1.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep1.TabIndex = 2;
            this.btnTestStep1.Text = "流程-1";
            this.btnTestStep1.UseVisualStyleBackColor = true;
            this.btnTestStep1.Click += new System.EventHandler(this.btnTestStep1_Click);
            // 
            // btnTestStep2
            // 
            this.btnTestStep2.Location = new System.Drawing.Point(589, 20);
            this.btnTestStep2.Name = "btnTestStep2";
            this.btnTestStep2.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep2.TabIndex = 2;
            this.btnTestStep2.Text = "流程-2";
            this.btnTestStep2.UseVisualStyleBackColor = true;
            this.btnTestStep2.Click += new System.EventHandler(this.btnTestStep2_Click);
            // 
            // btnTestStep3
            // 
            this.btnTestStep3.Enabled = false;
            this.btnTestStep3.Location = new System.Drawing.Point(729, 20);
            this.btnTestStep3.Name = "btnTestStep3";
            this.btnTestStep3.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep3.TabIndex = 2;
            this.btnTestStep3.Text = "測試-3";
            this.btnTestStep3.UseVisualStyleBackColor = true;
            this.btnTestStep3.Click += new System.EventHandler(this.btnTestStep3_Click);
            // 
            // btnTestStep4
            // 
            this.btnTestStep4.Enabled = false;
            this.btnTestStep4.Location = new System.Drawing.Point(869, 21);
            this.btnTestStep4.Name = "btnTestStep4";
            this.btnTestStep4.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep4.TabIndex = 2;
            this.btnTestStep4.Text = "測試-4";
            this.btnTestStep4.UseVisualStyleBackColor = true;
            this.btnTestStep4.Click += new System.EventHandler(this.btnTestStep4_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1131, 21);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(134, 72);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "存檔";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbWorkingDir
            // 
            this.tbWorkingDir.Location = new System.Drawing.Point(97, 22);
            this.tbWorkingDir.Name = "tbWorkingDir";
            this.tbWorkingDir.Size = new System.Drawing.Size(466, 22);
            this.tbWorkingDir.TabIndex = 4;
            this.tbWorkingDir.Text = "D:\\Temp";
            // 
            // btnWorkingDir
            // 
            this.btnWorkingDir.Location = new System.Drawing.Point(6, 21);
            this.btnWorkingDir.Name = "btnWorkingDir";
            this.btnWorkingDir.Size = new System.Drawing.Size(75, 23);
            this.btnWorkingDir.TabIndex = 5;
            this.btnWorkingDir.Text = "工作目錄";
            this.btnWorkingDir.UseVisualStyleBackColor = true;
            this.btnWorkingDir.Click += new System.EventHandler(this.btnWorkingDir_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(52, 195);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(43, 23);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "> |";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStopExec
            // 
            this.btnStopExec.Location = new System.Drawing.Point(3, 195);
            this.btnStopExec.Name = "btnStopExec";
            this.btnStopExec.Size = new System.Drawing.Size(43, 23);
            this.btnStopExec.TabIndex = 11;
            this.btnStopExec.Text = "口";
            this.btnStopExec.UseVisualStyleBackColor = true;
            this.btnStopExec.Click += new System.EventHandler(this.btnStopExec_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(101, 195);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(43, 23);
            this.btnRun.TabIndex = 12;
            this.btnRun.Text = "D";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbRecipeIndex
            // 
            this.tbRecipeIndex.Location = new System.Drawing.Point(150, 195);
            this.tbRecipeIndex.Name = "tbRecipeIndex";
            this.tbRecipeIndex.Size = new System.Drawing.Size(44, 22);
            this.tbRecipeIndex.TabIndex = 13;
            this.tbRecipeIndex.Text = "0";
            // 
            // btnSaveBenchAs
            // 
            this.btnSaveBenchAs.Location = new System.Drawing.Point(9, 79);
            this.btnSaveBenchAs.Name = "btnSaveBenchAs";
            this.btnSaveBenchAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveBenchAs.TabIndex = 0;
            this.btnSaveBenchAs.Text = "Save As";
            this.btnSaveBenchAs.UseVisualStyleBackColor = true;
            this.btnSaveBenchAs.Click += new System.EventHandler(this.btnSaveBenchAs_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnInstRead);
            this.groupBox3.Controls.Add(this.btnInstWrite);
            this.groupBox3.Controls.Add(this.btnRemoveInst);
            this.groupBox3.Controls.Add(this.btnInsertInstrument);
            this.groupBox3.Controls.Add(this.btnAddInst);
            this.groupBox3.Location = new System.Drawing.Point(6, 139);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(170, 213);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Instrument Operation";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSaveBenchAs);
            this.groupBox4.Controls.Add(this.btnInstLoad);
            this.groupBox4.Controls.Add(this.btnInstSave);
            this.groupBox4.Location = new System.Drawing.Point(3, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 127);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Bench Operation";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.button4);
            this.groupBox5.Controls.Add(this.btnSaveRecipe);
            this.groupBox5.Controls.Add(this.btnAddRecipe);
            this.groupBox5.Controls.Add(this.btnLoadRecipe);
            this.groupBox5.Controls.Add(this.lbRecipe);
            this.groupBox5.Controls.Add(this.tbRecipeIndex);
            this.groupBox5.Controls.Add(this.btnRun);
            this.groupBox5.Controls.Add(this.btnPause);
            this.groupBox5.Controls.Add(this.btnStopExec);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 223);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Recipe";
            // 
            // btnAddRecipe
            // 
            this.btnAddRecipe.Location = new System.Drawing.Point(6, 139);
            this.btnAddRecipe.Name = "btnAddRecipe";
            this.btnAddRecipe.Size = new System.Drawing.Size(54, 23);
            this.btnAddRecipe.TabIndex = 14;
            this.btnAddRecipe.Text = "Add";
            this.btnAddRecipe.UseVisualStyleBackColor = true;
            this.btnAddRecipe.Click += new System.EventHandler(this.btnAddRecipe_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(69, 139);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Remove";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(129, 139);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(54, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "Duplicate";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(129, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(57, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "Save As";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnExeRecipe);
            this.groupBox6.Controls.Add(this.btnAddProc);
            this.groupBox6.Controls.Add(this.btnRemoveProc);
            this.groupBox6.Controls.Add(this.btnInsertProc);
            this.groupBox6.Location = new System.Drawing.Point(6, 235);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 194);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Process";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnExecRecipe);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.btnImageFlash);
            this.tabPage4.Controls.Add(this.btnSelectImage);
            this.tabPage4.Controls.Add(this.tbImagePath);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1289, 435);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnImageFlash
            // 
            this.btnImageFlash.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnImageFlash.Location = new System.Drawing.Point(103, 48);
            this.btnImageFlash.Name = "btnImageFlash";
            this.btnImageFlash.Size = new System.Drawing.Size(466, 34);
            this.btnImageFlash.TabIndex = 5;
            this.btnImageFlash.Text = "燒錄";
            this.btnImageFlash.UseVisualStyleBackColor = true;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(13, 17);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(75, 23);
            this.btnSelectImage.TabIndex = 4;
            this.btnSelectImage.Text = "燒錄檔...";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            // 
            // tbImagePath
            // 
            this.tbImagePath.Location = new System.Drawing.Point(103, 19);
            this.tbImagePath.Name = "tbImagePath";
            this.tbImagePath.Size = new System.Drawing.Size(466, 22);
            this.tbImagePath.TabIndex = 3;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(12, 188);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 24;
            this.dataGridView3.Size = new System.Drawing.Size(1271, 241);
            this.dataGridView3.TabIndex = 6;
            this.dataGridView3.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView3_CellFormatting);
            // 
            // btnExecRecipe
            // 
            this.btnExecRecipe.Location = new System.Drawing.Point(301, 106);
            this.btnExecRecipe.Name = "btnExecRecipe";
            this.btnExecRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnExecRecipe.TabIndex = 8;
            this.btnExecRecipe.Text = "Execute";
            this.btnExecRecipe.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(207, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(113, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1315, 485);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnInstLoad;
        private System.Windows.Forms.Button btnInstSave;
        private System.Windows.Forms.Button btnInstWrite;
        private System.Windows.Forms.Button btnInstRead;
        private System.Windows.Forms.Button btnRemoveInst;
        private System.Windows.Forms.Button btnAddInst;
        private System.Windows.Forms.Button btnSaveRecipe;
        private System.Windows.Forms.Button btnLoadRecipe;
        private System.Windows.Forms.Button btnRemoveProc;
        private System.Windows.Forms.Button btnAddProc;
        private System.Windows.Forms.Button btnExeRecipe;
        private System.Windows.Forms.ListBox lbRecipe;
        private System.Windows.Forms.Button btnInsertProc;
        private System.Windows.Forms.Button btnInsertInstrument;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtFlashMessage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbDUTSerial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTestStep4;
        private System.Windows.Forms.Button btnTestStep3;
        private System.Windows.Forms.Button btnTestStep2;
        private System.Windows.Forms.Button btnTestStep1;
        private System.Windows.Forms.TextBox tbWorkingDir;
        private System.Windows.Forms.Button btnWorkingDir;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStopExec;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.TextBox tbRecipeIndex;
        private System.Windows.Forms.Button btnSaveBenchAs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnAddRecipe;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnImageFlash;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.TextBox tbImagePath;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button btnExecRecipe;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

