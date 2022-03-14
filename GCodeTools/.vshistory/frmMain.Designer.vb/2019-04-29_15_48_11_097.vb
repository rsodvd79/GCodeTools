<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelCommand = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonLoadGCode = New System.Windows.Forms.Button()
        Me.ButtonSaveOpenScad = New System.Windows.Forms.Button()
        Me.ButtonCreaSTL = New System.Windows.Forms.Button()
        Me.TextBoxGCode = New System.Windows.Forms.TextBox()
        Me.TextBoxOpenScad = New System.Windows.Forms.TextBox()
        Me.ProgressBarStato = New System.Windows.Forms.ProgressBar()
        Me.LabelLog = New System.Windows.Forms.Label()
        Me.ProgressBarRisultato = New System.Windows.Forms.ProgressBar()
        Me.ToolTipMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanelSetup = New System.Windows.Forms.TableLayoutPanel()
        Me.NumericUpDownProcessi = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NumericUpDownSegmenti = New System.Windows.Forms.NumericUpDown()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.TableLayoutPanelCommand.SuspendLayout()
        Me.TableLayoutPanelSetup.SuspendLayout()
        CType(Me.NumericUpDownProcessi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownSegmenti, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 1
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanelCommand, 0, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxGCode, 0, 1)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxOpenScad, 0, 2)
        Me.TableLayoutPanelMain.Controls.Add(Me.ProgressBarStato, 0, 5)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelLog, 0, 3)
        Me.TableLayoutPanelMain.Controls.Add(Me.ProgressBarRisultato, 0, 6)
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanelSetup, 0, 4)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 7
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(800, 450)
        Me.TableLayoutPanelMain.TabIndex = 0
        '
        'TableLayoutPanelCommand
        '
        Me.TableLayoutPanelCommand.ColumnCount = 3
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonLoadGCode, 0, 0)
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonSaveOpenScad, 1, 0)
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonCreaSTL, 2, 0)
        Me.TableLayoutPanelCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelCommand.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelCommand.Name = "TableLayoutPanelCommand"
        Me.TableLayoutPanelCommand.RowCount = 1
        Me.TableLayoutPanelCommand.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelCommand.Size = New System.Drawing.Size(794, 34)
        Me.TableLayoutPanelCommand.TabIndex = 0
        '
        'ButtonLoadGCode
        '
        Me.ButtonLoadGCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonLoadGCode.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLoadGCode.Name = "ButtonLoadGCode"
        Me.ButtonLoadGCode.Size = New System.Drawing.Size(258, 28)
        Me.ButtonLoadGCode.TabIndex = 0
        Me.ButtonLoadGCode.Text = "LOAD G-CODE"
        Me.ButtonLoadGCode.UseVisualStyleBackColor = True
        '
        'ButtonSaveOpenScad
        '
        Me.ButtonSaveOpenScad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSaveOpenScad.Location = New System.Drawing.Point(267, 3)
        Me.ButtonSaveOpenScad.Name = "ButtonSaveOpenScad"
        Me.ButtonSaveOpenScad.Size = New System.Drawing.Size(258, 28)
        Me.ButtonSaveOpenScad.TabIndex = 1
        Me.ButtonSaveOpenScad.Text = "SAVE OPEN SCAD"
        Me.ButtonSaveOpenScad.UseVisualStyleBackColor = True
        '
        'ButtonCreaSTL
        '
        Me.ButtonCreaSTL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonCreaSTL.Location = New System.Drawing.Point(531, 3)
        Me.ButtonCreaSTL.Name = "ButtonCreaSTL"
        Me.ButtonCreaSTL.Size = New System.Drawing.Size(260, 28)
        Me.ButtonCreaSTL.TabIndex = 2
        Me.ButtonCreaSTL.Text = "CREA STL"
        Me.ButtonCreaSTL.UseVisualStyleBackColor = True
        '
        'TextBoxGCode
        '
        Me.TextBoxGCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxGCode.Location = New System.Drawing.Point(3, 43)
        Me.TextBoxGCode.Multiline = True
        Me.TextBoxGCode.Name = "TextBoxGCode"
        Me.TextBoxGCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxGCode.Size = New System.Drawing.Size(794, 139)
        Me.TextBoxGCode.TabIndex = 1
        '
        'TextBoxOpenScad
        '
        Me.TextBoxOpenScad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxOpenScad.Location = New System.Drawing.Point(3, 188)
        Me.TextBoxOpenScad.Multiline = True
        Me.TextBoxOpenScad.Name = "TextBoxOpenScad"
        Me.TextBoxOpenScad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxOpenScad.Size = New System.Drawing.Size(794, 139)
        Me.TextBoxOpenScad.TabIndex = 2
        '
        'ProgressBarStato
        '
        Me.ProgressBarStato.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBarStato.Location = New System.Drawing.Point(3, 393)
        Me.ProgressBarStato.Name = "ProgressBarStato"
        Me.ProgressBarStato.Size = New System.Drawing.Size(794, 24)
        Me.ProgressBarStato.TabIndex = 3
        Me.ToolTipMain.SetToolTip(Me.ProgressBarStato, "Progrezzo Processi")
        '
        'LabelLog
        '
        Me.LabelLog.AutoSize = True
        Me.LabelLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelLog.Location = New System.Drawing.Point(3, 330)
        Me.LabelLog.Name = "LabelLog"
        Me.LabelLog.Size = New System.Drawing.Size(794, 30)
        Me.LabelLog.TabIndex = 4
        Me.LabelLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBarRisultato
        '
        Me.ProgressBarRisultato.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBarRisultato.Location = New System.Drawing.Point(3, 423)
        Me.ProgressBarRisultato.Name = "ProgressBarRisultato"
        Me.ProgressBarRisultato.Size = New System.Drawing.Size(794, 24)
        Me.ProgressBarRisultato.TabIndex = 7
        Me.ToolTipMain.SetToolTip(Me.ProgressBarRisultato, "Progresso greazione STL")
        '
        'TableLayoutPanelSetup
        '
        Me.TableLayoutPanelSetup.ColumnCount = 4
        Me.TableLayoutPanelSetup.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelSetup.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelSetup.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelSetup.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanelSetup.Controls.Add(Me.NumericUpDownProcessi, 1, 0)
        Me.TableLayoutPanelSetup.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanelSetup.Controls.Add(Me.Label2, 2, 0)
        Me.TableLayoutPanelSetup.Controls.Add(Me.NumericUpDownSegmenti, 3, 0)
        Me.TableLayoutPanelSetup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelSetup.Location = New System.Drawing.Point(3, 363)
        Me.TableLayoutPanelSetup.Name = "TableLayoutPanelSetup"
        Me.TableLayoutPanelSetup.RowCount = 1
        Me.TableLayoutPanelSetup.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelSetup.Size = New System.Drawing.Size(794, 24)
        Me.TableLayoutPanelSetup.TabIndex = 10
        '
        'NumericUpDownProcessi
        '
        Me.NumericUpDownProcessi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NumericUpDownProcessi.Dock = System.Windows.Forms.DockStyle.Left
        Me.NumericUpDownProcessi.Location = New System.Drawing.Point(201, 3)
        Me.NumericUpDownProcessi.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.NumericUpDownProcessi.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownProcessi.Name = "NumericUpDownProcessi"
        Me.NumericUpDownProcessi.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDownProcessi.TabIndex = 0
        Me.NumericUpDownProcessi.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(192, 26)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Processi OpenSCAD simultanei:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(399, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(192, 26)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Numero di segmenti in un blocco :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NumericUpDownSegmenti
        '
        Me.NumericUpDownSegmenti.Dock = System.Windows.Forms.DockStyle.Left
        Me.NumericUpDownSegmenti.Location = New System.Drawing.Point(597, 3)
        Me.NumericUpDownSegmenti.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NumericUpDownSegmenti.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDownSegmenti.Name = "NumericUpDownSegmenti"
        Me.NumericUpDownSegmenti.Size = New System.Drawing.Size(120, 20)
        Me.NumericUpDownSegmenti.TabIndex = 3
        Me.NumericUpDownSegmenti.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "G-Code Tools"
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.TableLayoutPanelMain.PerformLayout()
        Me.TableLayoutPanelCommand.ResumeLayout(False)
        Me.TableLayoutPanelSetup.ResumeLayout(False)
        Me.TableLayoutPanelSetup.PerformLayout()
        CType(Me.NumericUpDownProcessi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownSegmenti, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCommand As TableLayoutPanel
    Friend WithEvents ButtonLoadGCode As Button
    Friend WithEvents ButtonSaveOpenScad As Button
    Friend WithEvents TextBoxGCode As TextBox
    Friend WithEvents TextBoxOpenScad As TextBox
    Friend WithEvents ButtonCreaSTL As Button
    Friend WithEvents ProgressBarStato As ProgressBar
    Friend WithEvents LabelLog As Label
    Friend WithEvents ProgressBarRisultato As ProgressBar
    Friend WithEvents ToolTipMain As ToolTip
    Friend WithEvents TableLayoutPanelSetup As TableLayoutPanel
    Friend WithEvents NumericUpDownProcessi As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents NumericUpDownSegmenti As NumericUpDown
End Class
