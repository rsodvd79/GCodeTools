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
        Me.ButtonTest = New System.Windows.Forms.Button()
        Me.TextBoxGCode = New System.Windows.Forms.TextBox()
        Me.TextBoxOpenScad = New System.Windows.Forms.TextBox()
        Me.ProgressBarStato = New System.Windows.Forms.ProgressBar()
        Me.LabelLog = New System.Windows.Forms.Label()
        Me.TimerControllo = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.TableLayoutPanelCommand.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 1
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanelCommand, 0, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxGCode, 0, 1)
        Me.TableLayoutPanelMain.Controls.Add(Me.TextBoxOpenScad, 0, 2)
        Me.TableLayoutPanelMain.Controls.Add(Me.ProgressBarStato, 0, 4)
        Me.TableLayoutPanelMain.Controls.Add(Me.LabelLog, 0, 3)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 5
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
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
        Me.TableLayoutPanelCommand.Controls.Add(Me.ButtonTest, 2, 0)
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
        'ButtonTest
        '
        Me.ButtonTest.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonTest.Location = New System.Drawing.Point(531, 3)
        Me.ButtonTest.Name = "ButtonTest"
        Me.ButtonTest.Size = New System.Drawing.Size(260, 28)
        Me.ButtonTest.TabIndex = 2
        Me.ButtonTest.Text = "TEST"
        Me.ButtonTest.UseVisualStyleBackColor = True
        '
        'TextBoxGCode
        '
        Me.TextBoxGCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxGCode.Location = New System.Drawing.Point(3, 43)
        Me.TextBoxGCode.Multiline = True
        Me.TextBoxGCode.Name = "TextBoxGCode"
        Me.TextBoxGCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxGCode.Size = New System.Drawing.Size(794, 159)
        Me.TextBoxGCode.TabIndex = 1
        '
        'TextBoxOpenScad
        '
        Me.TextBoxOpenScad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxOpenScad.Location = New System.Drawing.Point(3, 208)
        Me.TextBoxOpenScad.Multiline = True
        Me.TextBoxOpenScad.Name = "TextBoxOpenScad"
        Me.TextBoxOpenScad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxOpenScad.Size = New System.Drawing.Size(794, 159)
        Me.TextBoxOpenScad.TabIndex = 2
        '
        'ProgressBarStato
        '
        Me.ProgressBarStato.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBarStato.Location = New System.Drawing.Point(3, 413)
        Me.ProgressBarStato.Name = "ProgressBarStato"
        Me.ProgressBarStato.Size = New System.Drawing.Size(794, 34)
        Me.ProgressBarStato.TabIndex = 3
        '
        'LabelLog
        '
        Me.LabelLog.AutoSize = True
        Me.LabelLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelLog.Location = New System.Drawing.Point(3, 370)
        Me.LabelLog.Name = "LabelLog"
        Me.LabelLog.Size = New System.Drawing.Size(794, 40)
        Me.LabelLog.TabIndex = 4
        Me.LabelLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TimerControllo
        '
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
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCommand As TableLayoutPanel
    Friend WithEvents ButtonLoadGCode As Button
    Friend WithEvents ButtonSaveOpenScad As Button
    Friend WithEvents TextBoxGCode As TextBox
    Friend WithEvents TextBoxOpenScad As TextBox
    Friend WithEvents ButtonTest As Button
    Friend WithEvents ProgressBarStato As ProgressBar
    Friend WithEvents TimerControllo As Timer
    Friend WithEvents LabelLog As Label
End Class
