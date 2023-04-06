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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_addInstrument = new System.Windows.Forms.Button();
            this.cb_instruments = new System.Windows.Forms.ComboBox();
            this.btn_addTimer = new System.Windows.Forms.Button();
            this.btn_addSTLINK = new System.Windows.Forms.Button();
            this.btn_addCAN = new System.Windows.Forms.Button();
            this.btn_addScpiLoad = new System.Windows.Forms.Button();
            this.gp_modbusRegs = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.btn_removeMBReg = new System.Windows.Forms.Button();
            this.btn_addMBReg = new System.Windows.Forms.Button();
            this.dgv_instrument_2 = new System.Windows.Forms.DataGridView();
            this.btn_addModbusDev = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSaveBenchAs = new System.Windows.Forms.Button();
            this.btnInstLoad = new System.Windows.Forms.Button();
            this.btnInstSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnInstRead = new System.Windows.Forms.Button();
            this.btnInstWrite = new System.Windows.Forms.Button();
            this.btnRemoveInst = new System.Windows.Forms.Button();
            this.btnInsertInstrument = new System.Windows.Forms.Button();
            this.btnAddInst = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnExeRecipe = new System.Windows.Forms.Button();
            this.btnAddProc = new System.Windows.Forms.Button();
            this.btnRemoveProc = new System.Windows.Forms.Button();
            this.btnInsertProc = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnSaveRecipe = new System.Windows.Forms.Button();
            this.btnAddRecipe = new System.Windows.Forms.Button();
            this.btnLoadRecipe = new System.Windows.Forms.Button();
            this.lbRecipe = new System.Windows.Forms.ListBox();
            this.tbRecipeIndex = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStopExec = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.tbDUTSerial = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnWorkingDir = new System.Windows.Forms.Button();
            this.tbWorkingDir = new System.Windows.Forms.TextBox();
            this.rtFlashMessage = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnTestStep4 = new System.Windows.Forms.Button();
            this.btnTestStep3 = new System.Windows.Forms.Button();
            this.btnTestStep2 = new System.Windows.Forms.Button();
            this.btnTestStep1 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnExecRecipe = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnImageFlash = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.tbImagePath = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btn_saveInterface = new System.Windows.Forms.Button();
            this.btn_loadInterface = new System.Windows.Forms.Button();
            this.btn_removeInterface = new System.Windows.Forms.Button();
            this.btn_addInterface = new System.Windows.Forms.Button();
            this.cb_interfaceType = new System.Windows.Forms.ComboBox();
            this.dgv_interface = new System.Windows.Forms.DataGridView();
            this.lvResult = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cellCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cellPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gp_modbusRegs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_instrument_2)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_interface)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(114, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(440, 423);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(217, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1066, 716);
            this.dataGridView2.TabIndex = 2;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView3_CellFormatting);
            this.dataGridView2.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseClick);
            this.dataGridView2.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_CellMouseDown);
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
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(6, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1297, 769);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_addInstrument);
            this.tabPage1.Controls.Add(this.cb_instruments);
            this.tabPage1.Controls.Add(this.btn_addTimer);
            this.tabPage1.Controls.Add(this.btn_addSTLINK);
            this.tabPage1.Controls.Add(this.btn_addCAN);
            this.tabPage1.Controls.Add(this.btn_addScpiLoad);
            this.tabPage1.Controls.Add(this.gp_modbusRegs);
            this.tabPage1.Controls.Add(this.dgv_instrument_2);
            this.tabPage1.Controls.Add(this.btn_addModbusDev);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1289, 557);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Instrument Definitions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_addInstrument
            // 
            this.btn_addInstrument.Location = new System.Drawing.Point(263, 454);
            this.btn_addInstrument.Name = "btn_addInstrument";
            this.btn_addInstrument.Size = new System.Drawing.Size(103, 23);
            this.btn_addInstrument.TabIndex = 20;
            this.btn_addInstrument.Text = "Add";
            this.btn_addInstrument.UseVisualStyleBackColor = true;
            this.btn_addInstrument.Click += new System.EventHandler(this.btn_addInstrument_Click);
            // 
            // cb_instruments
            // 
            this.cb_instruments.FormattingEnabled = true;
            this.cb_instruments.Items.AddRange(new object[] {
            "MODBUS Device",
            "SCPI LOAD",
            "DUT_CAN",
            "STLINK",
            "TIMER",
            "DUT_COM"});
            this.cb_instruments.Location = new System.Drawing.Point(127, 457);
            this.cb_instruments.Name = "cb_instruments";
            this.cb_instruments.Size = new System.Drawing.Size(121, 20);
            this.cb_instruments.TabIndex = 19;
            // 
            // btn_addTimer
            // 
            this.btn_addTimer.Location = new System.Drawing.Point(12, 484);
            this.btn_addTimer.Name = "btn_addTimer";
            this.btn_addTimer.Size = new System.Drawing.Size(96, 23);
            this.btn_addTimer.TabIndex = 18;
            this.btn_addTimer.Text = "TIMER";
            this.btn_addTimer.UseVisualStyleBackColor = true;
            this.btn_addTimer.Click += new System.EventHandler(this.btn_addTimer_Click);
            // 
            // btn_addSTLINK
            // 
            this.btn_addSTLINK.Location = new System.Drawing.Point(12, 455);
            this.btn_addSTLINK.Name = "btn_addSTLINK";
            this.btn_addSTLINK.Size = new System.Drawing.Size(96, 23);
            this.btn_addSTLINK.TabIndex = 17;
            this.btn_addSTLINK.Text = "STLINK";
            this.btn_addSTLINK.UseVisualStyleBackColor = true;
            this.btn_addSTLINK.Click += new System.EventHandler(this.btn_addSTLINK_Click);
            // 
            // btn_addCAN
            // 
            this.btn_addCAN.Location = new System.Drawing.Point(12, 426);
            this.btn_addCAN.Name = "btn_addCAN";
            this.btn_addCAN.Size = new System.Drawing.Size(96, 23);
            this.btn_addCAN.TabIndex = 16;
            this.btn_addCAN.Text = "CANBUS";
            this.btn_addCAN.UseVisualStyleBackColor = true;
            this.btn_addCAN.Click += new System.EventHandler(this.btn_addCAN_Click);
            // 
            // btn_addScpiLoad
            // 
            this.btn_addScpiLoad.Location = new System.Drawing.Point(12, 397);
            this.btn_addScpiLoad.Name = "btn_addScpiLoad";
            this.btn_addScpiLoad.Size = new System.Drawing.Size(96, 23);
            this.btn_addScpiLoad.TabIndex = 15;
            this.btn_addScpiLoad.Text = "SCPI_LOAD";
            this.btn_addScpiLoad.UseVisualStyleBackColor = true;
            this.btn_addScpiLoad.Click += new System.EventHandler(this.btn_addScpiLoad_Click);
            // 
            // gp_modbusRegs
            // 
            this.gp_modbusRegs.Controls.Add(this.button8);
            this.gp_modbusRegs.Controls.Add(this.btn_removeMBReg);
            this.gp_modbusRegs.Controls.Add(this.btn_addMBReg);
            this.gp_modbusRegs.Location = new System.Drawing.Point(593, 435);
            this.gp_modbusRegs.Name = "gp_modbusRegs";
            this.gp_modbusRegs.Size = new System.Drawing.Size(415, 100);
            this.gp_modbusRegs.TabIndex = 14;
            this.gp_modbusRegs.TabStop = false;
            this.gp_modbusRegs.Text = "MODBUS Registers";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(288, 32);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 2;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // btn_removeMBReg
            // 
            this.btn_removeMBReg.Location = new System.Drawing.Point(143, 33);
            this.btn_removeMBReg.Name = "btn_removeMBReg";
            this.btn_removeMBReg.Size = new System.Drawing.Size(75, 23);
            this.btn_removeMBReg.TabIndex = 1;
            this.btn_removeMBReg.Text = "Remove";
            this.btn_removeMBReg.UseVisualStyleBackColor = true;
            this.btn_removeMBReg.Click += new System.EventHandler(this.btn_removeMBReg_Click);
            // 
            // btn_addMBReg
            // 
            this.btn_addMBReg.Location = new System.Drawing.Point(35, 33);
            this.btn_addMBReg.Name = "btn_addMBReg";
            this.btn_addMBReg.Size = new System.Drawing.Size(75, 23);
            this.btn_addMBReg.TabIndex = 0;
            this.btn_addMBReg.Text = "ADD";
            this.btn_addMBReg.UseVisualStyleBackColor = true;
            this.btn_addMBReg.Click += new System.EventHandler(this.btn_addMBReg_Click);
            // 
            // dgv_instrument_2
            // 
            this.dgv_instrument_2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_instrument_2.Location = new System.Drawing.Point(593, 6);
            this.dgv_instrument_2.Name = "dgv_instrument_2";
            this.dgv_instrument_2.RowTemplate.Height = 24;
            this.dgv_instrument_2.Size = new System.Drawing.Size(415, 423);
            this.dgv_instrument_2.TabIndex = 13;
            // 
            // btn_addModbusDev
            // 
            this.btn_addModbusDev.Location = new System.Drawing.Point(12, 368);
            this.btn_addModbusDev.Name = "btn_addModbusDev";
            this.btn_addModbusDev.Size = new System.Drawing.Size(96, 23);
            this.btn_addModbusDev.TabIndex = 12;
            this.btn_addModbusDev.Text = "MODBUS Dev";
            this.btn_addModbusDev.UseVisualStyleBackColor = true;
            this.btn_addModbusDev.Click += new System.EventHandler(this.btn_addModbusDev_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSaveBenchAs);
            this.groupBox4.Controls.Add(this.btnInstLoad);
            this.groupBox4.Controls.Add(this.btnInstSave);
            this.groupBox4.Location = new System.Drawing.Point(3, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(105, 127);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Bench Operation";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnInstRead);
            this.groupBox3.Controls.Add(this.btnInstWrite);
            this.groupBox3.Controls.Add(this.btnRemoveInst);
            this.groupBox3.Controls.Add(this.btnInsertInstrument);
            this.groupBox3.Controls.Add(this.btnAddInst);
            this.groupBox3.Location = new System.Drawing.Point(6, 139);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(102, 213);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Instrument Operation";
            // 
            // btnInstRead
            // 
            this.btnInstRead.Location = new System.Drawing.Point(6, 38);
            this.btnInstRead.Name = "btnInstRead";
            this.btnInstRead.Size = new System.Drawing.Size(75, 23);
            this.btnInstRead.TabIndex = 2;
            this.btnInstRead.Text = "Read";
            this.btnInstRead.UseVisualStyleBackColor = true;
            this.btnInstRead.Click += new System.EventHandler(this.btnInstRead_Click);
            // 
            // btnInstWrite
            // 
            this.btnInstWrite.Location = new System.Drawing.Point(6, 67);
            this.btnInstWrite.Name = "btnInstWrite";
            this.btnInstWrite.Size = new System.Drawing.Size(75, 23);
            this.btnInstWrite.TabIndex = 3;
            this.btnInstWrite.Text = "Write";
            this.btnInstWrite.UseVisualStyleBackColor = true;
            this.btnInstWrite.Click += new System.EventHandler(this.btnInstWrite_Click);
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
            // btnInsertInstrument
            // 
            this.btnInsertInstrument.Location = new System.Drawing.Point(6, 125);
            this.btnInsertInstrument.Name = "btnInsertInstrument";
            this.btnInsertInstrument.Size = new System.Drawing.Size(75, 23);
            this.btnInsertInstrument.TabIndex = 8;
            this.btnInsertInstrument.Text = "Insert";
            this.btnInsertInstrument.UseVisualStyleBackColor = true;
            this.btnInsertInstrument.Click += new System.EventHandler(this.btnInsertInstrument_Click);
            // 
            // btnAddInst
            // 
            this.btnAddInst.Location = new System.Drawing.Point(6, 96);
            this.btnAddInst.Name = "btnAddInst";
            this.btnAddInst.Size = new System.Drawing.Size(75, 23);
            this.btnAddInst.TabIndex = 6;
            this.btnAddInst.Text = "Add";
            this.btnAddInst.UseVisualStyleBackColor = true;
            this.btnAddInst.Click += new System.EventHandler(this.btnAddInst_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1289, 743);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Recipe Definitions";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(129, 139);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(54, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "Duplicate";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(69, 139);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Remove";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
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
            // tbRecipeIndex
            // 
            this.tbRecipeIndex.Location = new System.Drawing.Point(150, 195);
            this.tbRecipeIndex.Name = "tbRecipeIndex";
            this.tbRecipeIndex.Size = new System.Drawing.Size(44, 22);
            this.tbRecipeIndex.TabIndex = 13;
            this.tbRecipeIndex.Text = "0";
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
            // btnPause
            // 
            this.btnPause.AccessibleDescription = "Tooltip";
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox8);
            this.tabPage3.Controls.Add(this.lvResult);
            this.tabPage3.Controls.Add(this.rtFlashMessage);
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1289, 743);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DUT測試";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnAbort);
            this.groupBox8.Controls.Add(this.tbDUTSerial);
            this.groupBox8.Controls.Add(this.btnSave);
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox8.Location = new System.Drawing.Point(6, 214);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(1274, 71);
            this.groupBox8.TabIndex = 8;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "步驟-3: 掃瞄序號-存檔";
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(970, 21);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(181, 35);
            this.btnAbort.TabIndex = 3;
            this.btnAbort.Text = "停止測試";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // tbDUTSerial
            // 
            this.tbDUTSerial.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbDUTSerial.Location = new System.Drawing.Point(229, 21);
            this.tbDUTSerial.Name = "tbDUTSerial";
            this.tbDUTSerial.Size = new System.Drawing.Size(451, 36);
            this.tbDUTSerial.TabIndex = 1;
            this.tbDUTSerial.Text = "12345";
            this.tbDUTSerial.FontChanged += new System.EventHandler(this.tbDUTSerial_FontChanged);
            this.tbDUTSerial.Enter += new System.EventHandler(this.tbDUTSerial_Enter);
            this.tbDUTSerial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbDUTSerial_KeyUp);
            this.tbDUTSerial.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbDUTSerial_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(686, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(211, 36);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "存檔";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "掃瞄/輸入序號";
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(3, 291);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 24;
            this.dataGridView3.Size = new System.Drawing.Size(1277, 253);
            this.dataGridView3.TabIndex = 6;
            this.dataGridView3.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView3_CellFormatting);
            // 
            // groupBox2
            // 
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(6, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1277, 105);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "步驟-2 測試";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnWorkingDir);
            this.groupBox1.Controls.Add(this.tbWorkingDir);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(6, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1277, 86);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "步驟-1 : 設定存檔路徑及序號";
            // 
            // btnWorkingDir
            // 
            this.btnWorkingDir.Location = new System.Drawing.Point(16, 26);
            this.btnWorkingDir.Name = "btnWorkingDir";
            this.btnWorkingDir.Size = new System.Drawing.Size(118, 23);
            this.btnWorkingDir.TabIndex = 5;
            this.btnWorkingDir.Text = "工作目錄";
            this.btnWorkingDir.UseVisualStyleBackColor = true;
            this.btnWorkingDir.Click += new System.EventHandler(this.btnWorkingDir_Click);
            // 
            // tbWorkingDir
            // 
            this.tbWorkingDir.Location = new System.Drawing.Point(140, 26);
            this.tbWorkingDir.Name = "tbWorkingDir";
            this.tbWorkingDir.Size = new System.Drawing.Size(420, 27);
            this.tbWorkingDir.TabIndex = 4;
            this.tbWorkingDir.Text = "D:\\Temp";
            // 
            // rtFlashMessage
            // 
            this.rtFlashMessage.Location = new System.Drawing.Point(510, 550);
            this.rtFlashMessage.Name = "rtFlashMessage";
            this.rtFlashMessage.Size = new System.Drawing.Size(770, 145);
            this.rtFlashMessage.TabIndex = 3;
            this.rtFlashMessage.Text = "1. 燒錄後須留意板上的綠色LED灯應該在閃爍!\n2. 請執行\"ALL\"進行全功能測試, 若單項功能測試未通過, 可單項獨立重測.\n3. 完成後,掃瞄條碼可自動儲" +
    "存測試結果, 若為手動輸入, 請完成後按\"ENTER\"鍵.\n4. 測試完成請檢查Result欄位, \"VALID\"打勾者需為\"PASS\"才算成功.\n5. 在對應" +
    "的按鈕上按MOUSE右鍵, 可以切換至該項流程的測項及結果.\n";
            this.rtFlashMessage.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnTestStep4);
            this.tabPage4.Controls.Add(this.btnTestStep3);
            this.tabPage4.Controls.Add(this.btnTestStep2);
            this.tabPage4.Controls.Add(this.btnTestStep1);
            this.tabPage4.Controls.Add(this.groupBox7);
            this.tabPage4.Controls.Add(this.btnExecRecipe);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.btnImageFlash);
            this.tabPage4.Controls.Add(this.btnSelectImage);
            this.tabPage4.Controls.Add(this.tbImagePath);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1289, 557);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnTestStep4
            // 
            this.btnTestStep4.Location = new System.Drawing.Point(627, 355);
            this.btnTestStep4.Name = "btnTestStep4";
            this.btnTestStep4.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep4.TabIndex = 10;
            this.btnTestStep4.Text = "測試-4";
            this.btnTestStep4.UseVisualStyleBackColor = true;
            this.btnTestStep4.Click += new System.EventHandler(this.btnTestStep4_Click_1);
            // 
            // btnTestStep3
            // 
            this.btnTestStep3.Enabled = false;
            this.btnTestStep3.Location = new System.Drawing.Point(487, 354);
            this.btnTestStep3.Name = "btnTestStep3";
            this.btnTestStep3.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep3.TabIndex = 11;
            this.btnTestStep3.Text = "測試-3";
            this.btnTestStep3.UseVisualStyleBackColor = true;
            // 
            // btnTestStep2
            // 
            this.btnTestStep2.Location = new System.Drawing.Point(347, 354);
            this.btnTestStep2.Name = "btnTestStep2";
            this.btnTestStep2.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep2.TabIndex = 12;
            this.btnTestStep2.Text = "流程-2";
            this.btnTestStep2.UseVisualStyleBackColor = true;
            // 
            // btnTestStep1
            // 
            this.btnTestStep1.Location = new System.Drawing.Point(207, 354);
            this.btnTestStep1.Name = "btnTestStep1";
            this.btnTestStep1.Size = new System.Drawing.Size(134, 72);
            this.btnTestStep1.TabIndex = 13;
            this.btnTestStep1.Text = "流程-1";
            this.btnTestStep1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(82, 166);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(924, 100);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "groupBox7";
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
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btn_saveInterface);
            this.tabPage5.Controls.Add(this.btn_loadInterface);
            this.tabPage5.Controls.Add(this.btn_removeInterface);
            this.tabPage5.Controls.Add(this.btn_addInterface);
            this.tabPage5.Controls.Add(this.cb_interfaceType);
            this.tabPage5.Controls.Add(this.dgv_interface);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1289, 557);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Interface Definition";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btn_saveInterface
            // 
            this.btn_saveInterface.Location = new System.Drawing.Point(606, 29);
            this.btn_saveInterface.Name = "btn_saveInterface";
            this.btn_saveInterface.Size = new System.Drawing.Size(75, 23);
            this.btn_saveInterface.TabIndex = 5;
            this.btn_saveInterface.Text = "Save";
            this.btn_saveInterface.UseVisualStyleBackColor = true;
            this.btn_saveInterface.Click += new System.EventHandler(this.btn_saveInterface_Click);
            // 
            // btn_loadInterface
            // 
            this.btn_loadInterface.Location = new System.Drawing.Point(479, 29);
            this.btn_loadInterface.Name = "btn_loadInterface";
            this.btn_loadInterface.Size = new System.Drawing.Size(75, 23);
            this.btn_loadInterface.TabIndex = 4;
            this.btn_loadInterface.Text = "Load";
            this.btn_loadInterface.UseVisualStyleBackColor = true;
            this.btn_loadInterface.Click += new System.EventHandler(this.btn_loadInterface_Click);
            // 
            // btn_removeInterface
            // 
            this.btn_removeInterface.Location = new System.Drawing.Point(379, 30);
            this.btn_removeInterface.Name = "btn_removeInterface";
            this.btn_removeInterface.Size = new System.Drawing.Size(75, 23);
            this.btn_removeInterface.TabIndex = 3;
            this.btn_removeInterface.Text = "Remove";
            this.btn_removeInterface.UseVisualStyleBackColor = true;
            this.btn_removeInterface.Click += new System.EventHandler(this.btn_removeInterface_Click);
            // 
            // btn_addInterface
            // 
            this.btn_addInterface.Location = new System.Drawing.Point(273, 30);
            this.btn_addInterface.Name = "btn_addInterface";
            this.btn_addInterface.Size = new System.Drawing.Size(75, 23);
            this.btn_addInterface.TabIndex = 2;
            this.btn_addInterface.Text = "Add";
            this.btn_addInterface.UseVisualStyleBackColor = true;
            this.btn_addInterface.Click += new System.EventHandler(this.btn_addInterface_Click);
            // 
            // cb_interfaceType
            // 
            this.cb_interfaceType.FormattingEnabled = true;
            this.cb_interfaceType.Items.AddRange(new object[] {
            "MODBUS",
            "USB-CAN",
            "NATIVE"});
            this.cb_interfaceType.Location = new System.Drawing.Point(132, 32);
            this.cb_interfaceType.Name = "cb_interfaceType";
            this.cb_interfaceType.Size = new System.Drawing.Size(121, 20);
            this.cb_interfaceType.TabIndex = 1;
            // 
            // dgv_interface
            // 
            this.dgv_interface.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_interface.Location = new System.Drawing.Point(195, 97);
            this.dgv_interface.Name = "dgv_interface";
            this.dgv_interface.RowTemplate.Height = 24;
            this.dgv_interface.Size = new System.Drawing.Size(809, 201);
            this.dgv_interface.TabIndex = 0;
            // 
            // lvResult
            // 
            this.lvResult.HideSelection = false;
            this.lvResult.Location = new System.Drawing.Point(6, 550);
            this.lvResult.Name = "lvResult";
            this.lvResult.Size = new System.Drawing.Size(498, 145);
            this.lvResult.TabIndex = 9;
            this.lvResult.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cellCopy,
            this.cellPaste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(105, 48);
            // 
            // cellCopy
            // 
            this.cellCopy.Name = "cellCopy";
            this.cellCopy.Size = new System.Drawing.Size(104, 22);
            this.cellCopy.Text = "Copy";
            this.cellCopy.Click += new System.EventHandler(this.cellCopy_Click);
            // 
            // cellPaste
            // 
            this.cellPaste.Name = "cellPaste";
            this.cellPaste.Size = new System.Drawing.Size(104, 22);
            this.cellPaste.Text = "Paste";
            this.cellPaste.Click += new System.EventHandler(this.cellPaste_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 749);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1315, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1315, 771);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ATE";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gp_modbusRegs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_instrument_2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_interface)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cellCopy;
        private System.Windows.Forms.ToolStripMenuItem cellPaste;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnTestStep4;
        private System.Windows.Forms.Button btnTestStep3;
        private System.Windows.Forms.Button btnTestStep2;
        private System.Windows.Forms.Button btnTestStep1;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dgv_interface;
        private System.Windows.Forms.Button btn_saveInterface;
        private System.Windows.Forms.Button btn_loadInterface;
        private System.Windows.Forms.Button btn_removeInterface;
        private System.Windows.Forms.Button btn_addInterface;
        private System.Windows.Forms.ComboBox cb_interfaceType;
        private System.Windows.Forms.Button btn_addModbusDev;
        private System.Windows.Forms.DataGridView dgv_instrument_2;
        private System.Windows.Forms.GroupBox gp_modbusRegs;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btn_removeMBReg;
        private System.Windows.Forms.Button btn_addMBReg;
        private System.Windows.Forms.Button btn_addScpiLoad;
        private System.Windows.Forms.Button btn_addSTLINK;
        private System.Windows.Forms.Button btn_addCAN;
        private System.Windows.Forms.Button btn_addTimer;
        private System.Windows.Forms.Button btn_addInstrument;
        private System.Windows.Forms.ComboBox cb_instruments;
    }
}

