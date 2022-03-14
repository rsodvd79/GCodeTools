Imports System.ComponentModel
Imports System.Globalization
Imports System.Text

Public Class frmMain
    Dim dblUgello As Double = 0.4
    Dim dblLayer As Double = 0.2

    Private Sub ButtonLoadGCode_Click(sender As Object, e As EventArgs) Handles ButtonLoadGCode.Click
        TextBoxGCode.Text = String.Empty
        TextBoxOpenScad.Text = String.Empty

        Dim frmX As New OpenFileDialog With {
            .Filter = "*.gcode|*.gcode"
        }

        If frmX.ShowDialog(Me) = DialogResult.OK Then
            TextBoxGCode.Text = IO.File.ReadAllText(frmX.FileName)

        End If

    End Sub

    Private Sub ButtonSaveOpenScad_Click(sender As Object, e As EventArgs) Handles ButtonSaveOpenScad.Click
        Dim frmX As New SaveFileDialog With {
            .Filter = "*.scad|*.scad"
        }

        If frmX.ShowDialog(Me) = DialogResult.OK Then
            GCodeToOpenScad()
            IO.File.WriteAllText(frmX.FileName, TextBoxOpenScad.Text)

        End If

    End Sub

    Private Class clsSegmento
        Public Property Livello As Integer = 0
        Public Property Layer As Double = 0

        Public Class clsXYZ
            Public Property X As Double = 0
            Public Property Y As Double = 0
            Public Property Z As Double = 0

            Public Sub New(_x As Double, _y As Double, _z As Double)
                X = _x
                Y = _y
                Z = _z

            End Sub

            Public ReadOnly Property aStringaSemplice As String
                Get
                    Return $"{X.ToString(CultureInfo.InvariantCulture)},{Y.ToString(CultureInfo.InvariantCulture)},{Z.ToString(CultureInfo.InvariantCulture)}"

                End Get
            End Property

            Public ReadOnly Property aStringaComplessa As String
                Get
                    Return $"[{aStringaSemplice()}]"

                End Get
            End Property

        End Class

        Public Property Origine As clsXYZ
        Public Property Destinazione As clsXYZ

        Public ReadOnly Property Angolo As Double
            Get
                If Destinazione.X >= Origine.X Then
                    Return Math.Atan((Destinazione.Y - Origine.Y) / (Destinazione.X - Origine.X)) * 180 / Math.PI

                Else
                    Return (Math.Atan((Origine.Y - Destinazione.Y) / (Origine.X - Destinazione.X)) * 180 / Math.PI) + 180

                End If

            End Get
        End Property

        Public ReadOnly Property Distanza As Double
            Get
                Return Math.Sqrt((Math.Abs(Destinazione.Y - Origine.Y) ^ 2) + (Math.Abs(Destinazione.X - Origine.X) ^ 2))

            End Get
        End Property

        Public ReadOnly Property aStringaSemplice As String
            Get
                Return $"{Origine.aStringaSemplice()},{Destinazione.aStringaSemplice()}"

            End Get
        End Property

        Public ReadOnly Property aStringaComplessa As String
            Get
                Return $"[{Origine.aStringaComplessa()},{Destinazione.aStringaComplessa()}]"

            End Get
        End Property

        Public ReadOnly Property aStringaComplessaAD As String
            Get
                Return $"[{Origine.aStringaComplessa()},{Angolo.ToString(CultureInfo.InvariantCulture)},{Distanza.ToString(CultureInfo.InvariantCulture)}]"

            End Get
        End Property

        Public Sub New(_livello As Integer, _layer As Double, _x1 As Double, _y1 As Double, _z1 As Double, _x2 As Double, _y2 As Double, _z2 As Double)
            Livello = _livello
            Layer = _layer

            Origine = New clsXYZ(_x1, _y1, _z1)

            Destinazione = New clsXYZ(_x2, _y2, _z2)

        End Sub

    End Class

    Private Segmenti As List(Of clsSegmento)

    Private Sub GCodeToSegmenti()

        Segmenti = New List(Of clsSegmento)

        Dim dblOX As Double = 0
        Dim dblOY As Double = 0
        Dim dblOZ As Double = 0
        Dim dblOE As Double = 0
        Dim dblOF As Double = 0

        Dim dblDX As Double = 0
        Dim dblDY As Double = 0
        Dim dblDZ As Double = 0
        Dim dblDE As Double = 0
        Dim dblDF As Double = 0

        Dim intLi As Integer = 0
        Dim dblLa As Double = dblLayer

        Dim blnPA As Boolean = True

        For Each strR As String In TextBoxGCode.Text.Replace(vbLf, String.Empty).Split(vbCr)
            If strR.Contains(";") Then
                strR = strR.Split(";")(0)

            End If

            strR = strR.Trim.ToUpper

            If Not String.IsNullOrEmpty(strR) Then

                Dim strP() As String = strR.Split(" ")

                Select Case strP(0)
                    Case "G90"
                        blnPA = True

                    Case "G91"
                        blnPA = False

                    Case "G0", "G92"
                        For intI As Integer = 1 To strP.Count - 1
                            Dim strV As String = strP(intI)

                            If strV.StartsWith("X") Then
                                dblDX = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOX = dblDX

                            ElseIf strV.StartsWith("Y") Then
                                dblDY = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOY = dblDY

                            ElseIf strV.StartsWith("Z") Then
                                dblDZ = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                                If dblOZ < dblDZ Then
                                    intLi += 1
                                    dblLa = Math.Abs(dblDZ - dblOZ)

                                ElseIf dblOZ > dblDZ Then
                                    intLi = 1
                                    dblLa = dblDZ

                                End If

                                dblOZ = dblDZ

                            ElseIf strV.StartsWith("E") Then
                                dblDE = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOE = dblDE

                            ElseIf strV.StartsWith("F") Then
                                dblDF = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
                                dblOF = dblDF

                            End If

                        Next

                    Case "G1"
                        For intI As Integer = 1 To strP.Count - 1
                            Dim strV As String = strP(intI)

                            If strV.StartsWith("X") Then
                                dblDX = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("Y") Then
                                dblDY = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("Z") Then
                                dblDZ = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("E") Then
                                dblDE = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            ElseIf strV.StartsWith("F") Then
                                dblDF = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

                            End If

                        Next

                        If dblOZ < dblDZ Then
                            intLi += 1
                            dblLa = Math.Abs(dblDZ - dblOZ)

                        ElseIf dblOZ > dblDZ Then
                            intLi = 1
                            dblLa = dblDZ

                        End If

                        Dim Distanza As Double = Math.Sqrt((Math.Abs(dblDY - dblOY) ^ 2) + (Math.Abs(dblDX - dblOX) ^ 2))

                        If Distanza > 0 AndAlso dblDE > 0 Then
                            Segmenti.Add(New clsSegmento(intLi, dblLa, dblOX, dblOY, dblOZ, dblDX, dblDY, dblDZ))

                        End If

                        dblOX = dblDX
                        dblOY = dblDY
                        dblOZ = dblDZ
                        dblOE = dblDE
                        dblOF = dblDF

                End Select

            End If

        Next

    End Sub

    Private Sub GCodeToOpenScad()

        GCodeToSegmenti()

        Dim stbR As New StringBuilder()

        stbR.AppendLine("$fn=10;")
        stbR.AppendLine("")
        stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
        stbR.AppendLine($"Layer={dblLayer.ToString(CultureInfo.InvariantCulture)} + 0.001;")
        stbR.AppendLine("")
        'stbR.AppendLine("ModoDrw = 3;")
        'stbR.AppendLine("")
        stbR.AppendLine("module drw(xyz) {")
        'stbR.AppendLine("echo(xyz);")
        'stbR.AppendLine("    if (ModoDrw==1) {")
        'stbR.AppendLine("        hull() {")
        'stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.AppendLine("        };")
        'stbR.AppendLine("    } else if (ModoDrw==2) {")
        'stbR.AppendLine("        translate([xyz[3], xyz[4], xyz[5]]) cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.AppendLine("    } else if (ModoDrw==3) {")
        'stbR.AppendLine("")
        stbR.AppendLine("        dy = abs(xyz[4] - xyz[1]);")
        stbR.AppendLine("        dx = abs(xyz[3] - xyz[0]);")
        stbR.AppendLine("")
        stbR.AppendLine("        distanza = sqrt((dy * dy) + (dx * dx));")
        stbR.AppendLine("")
        stbR.AppendLine("        if (xyz[3] >= xyz[0]) {")
        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, atan((xyz[4] - xyz[1]) / (xyz[3] - xyz[0]))]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
        stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
        stbR.AppendLine("        } else {")
        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, (atan((xyz[1] - xyz[4]) / (xyz[0] - xyz[3])) + 180.0)]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
        stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
        stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
        stbR.AppendLine("     }")
        'stbR.AppendLine("")
        'stbR.AppendLine("    };")
        stbR.AppendLine("};")

        'stbR.AppendLine("module drw(x1, y1, z1, x2, y2, z2) {")
        'stbR.AppendLine("module drw(xyz) {")
        'stbR.AppendLine("hull() {")
        ''stbR.Append($"translate([x1, y1, z1]) ") : stbR.AppendLine($"cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.Append($"translate([xyz[0], xyz[1], xyz[2]]) ") : stbR.AppendLine($"cylinder(h=Layer,d=Ugello,center=true);")
        ''stbR.Append($"translate([x2, y2, z2]) ") : stbR.AppendLine($"cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.Append($"translate([xyz[3], xyz[4], xyz[5]]) ") : stbR.AppendLine($"cylinder(h=Layer,d=Ugello,center=true);")
        'stbR.AppendLine("};")
        'stbR.AppendLine("};")

        'stbR.AppendLine("module pla(lunghezza, angolo, x, y, z){")
        'stbR.AppendLine("translate([x, y, z])")
        'stbR.AppendLine("rotate([0, 0, angolo])")
        'stbR.AppendLine("cube([lunghezza, Ugello, Layer]);")
        ''stbR.AppendLine("rotate([0, 90, 0])")
        ''stbR.AppendLine("cylinder(h=lunghezza, d=Ugello);")
        'stbR.AppendLine("};")
        stbR.AppendLine("")
        'stbR.AppendLine("union() {")
        stbR.Append("gcd = [")

        Dim blnPrimo As Boolean = True

        For Each sg As clsSegmento In Segmenti
            If blnPrimo Then
                blnPrimo = False

            Else
                stbR.AppendLine($",")

            End If

            stbR.Append($"[{sg.aStringaSemplice()}]")

        Next


        'Dim dblOX As Double = 0
        'Dim dblOY As Double = 0
        'Dim dblOZ As Double = 0
        'Dim dblOE As Double = 0
        'Dim dblOF As Double = 0

        'Dim dblDX As Double = 0
        'Dim dblDY As Double = 0
        'Dim dblDZ As Double = 0
        'Dim dblDE As Double = 0
        'Dim dblDF As Double = 0

        'Dim blnPA As Boolean = True


        'Dim intCnt As Integer = 0

        'For Each strR As String In TextBoxGCode.Text.Replace(vbLf, String.Empty).Split(vbCr)
        '    If strR.Contains(";") Then
        '        strR = strR.Split(";")(0)

        '    End If

        '    strR = strR.Trim.ToUpper

        '    If Not String.IsNullOrEmpty(strR) Then
        '        'stbR.AppendLine($"// {strR}")

        '        Dim strP() As String = strR.Split(" ")

        '        Select Case strP(0)
        '            Case "G90"
        '                blnPA = True

        '            Case "G91"
        '                blnPA = False

        '            Case "G0", "G92"
        '                For intI As Integer = 1 To strP.Count - 1
        '                    Dim strV As String = strP(intI)

        '                    If strV.StartsWith("X") Then
        '                        dblDX = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
        '                        dblOX = dblDX

        '                    ElseIf strV.StartsWith("Y") Then
        '                        dblDY = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
        '                        dblOY = dblDY

        '                    ElseIf strV.StartsWith("Z") Then
        '                        dblDZ = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
        '                        dblOZ = dblDZ

        '                    ElseIf strV.StartsWith("E") Then
        '                        dblDE = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
        '                        dblOE = dblDE

        '                    ElseIf strV.StartsWith("F") Then
        '                        dblDF = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)
        '                        dblOF = dblDF

        '                    End If

        '                Next

        '            Case "G1"
        '                For intI As Integer = 1 To strP.Count - 1
        '                    Dim strV As String = strP(intI)

        '                    If strV.StartsWith("X") Then
        '                        dblDX = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

        '                    ElseIf strV.StartsWith("Y") Then
        '                        dblDY = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

        '                    ElseIf strV.StartsWith("Z") Then
        '                        dblDZ = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

        '                    ElseIf strV.StartsWith("E") Then
        '                        dblDE = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

        '                    ElseIf strV.StartsWith("F") Then
        '                        dblDF = Double.Parse(strV.Substring(1), CultureInfo.InvariantCulture)

        '                    End If

        '                Next

        '                Dim Distanza As Double = Math.Sqrt((Math.Abs(dblDY - dblOY) ^ 2) + (Math.Abs(dblDX - dblOX) ^ 2))

        '                If Distanza > 0 AndAlso dblDE > 0 Then
        '                    intCnt += 1

        '                    If blnPrimo Then
        '                        blnPrimo = False

        '                    Else
        '                        stbR.AppendLine($",")

        '                    End If

        '                    stbR.Append($"[{dblOX.ToString(CultureInfo.InvariantCulture)},{dblOY.ToString(CultureInfo.InvariantCulture)},{dblOZ.ToString(CultureInfo.InvariantCulture)},{dblDX.ToString(CultureInfo.InvariantCulture)},{dblDY.ToString(CultureInfo.InvariantCulture)},{dblDZ.ToString(CultureInfo.InvariantCulture)}]")

        '                    '  stbR.AppendLine($"drw({dblOX.ToString(CultureInfo.InvariantCulture)},{dblOY.ToString(CultureInfo.InvariantCulture)},{dblOZ.ToString(CultureInfo.InvariantCulture)},{dblDX.ToString(CultureInfo.InvariantCulture)},{dblDY.ToString(CultureInfo.InvariantCulture)},{dblDZ.ToString(CultureInfo.InvariantCulture)});")

        '                    'Dim Angolo As Double = 0

        '                    'If dblDX >= dblOX Then
        '                    '    Angolo = Math.Atan((dblDY - dblOY) / (dblDX - dblOX)) * 180 / Math.PI

        '                    'Else
        '                    '    Angolo = (Math.Atan((dblOY - dblDY) / (dblOX - dblDX)) * 180 / Math.PI) + 180

        '                    'End If

        '                    ' stbR.AppendLine($"pla({Distanza.ToString(CultureInfo.InvariantCulture)},{Angolo.ToString(CultureInfo.InvariantCulture)},{dblOX.ToString(CultureInfo.InvariantCulture)},{dblOY.ToString(CultureInfo.InvariantCulture)},{dblOZ.ToString(CultureInfo.InvariantCulture)});")

        '                    'stbR.Append($"translate([{dblOX.ToString(CultureInfo.InvariantCulture)},{dblOY.ToString(CultureInfo.InvariantCulture)},{dblOZ.ToString(CultureInfo.InvariantCulture)}]) ")
        '                    'stbR.Append($"rotate([0,0,{Angolo.ToString(CultureInfo.InvariantCulture)}]) ")
        '                    ' stbR.AppendLine($"cube([{Distanza.ToString(CultureInfo.InvariantCulture)},{dblUgello.ToString(CultureInfo.InvariantCulture)},{dblLayer.ToString(CultureInfo.InvariantCulture)}]);")

        '                    'stbR.Append($"rotate([0,90,0]) ")
        '                    'stbR.AppendLine($"cylinder(h={Distanza.ToString(CultureInfo.InvariantCulture)},d=Ugello);")

        '                    'stbR.AppendLine("hull() {")

        '                    'stbR.Append($"translate([{dblOX.ToString(CultureInfo.InvariantCulture)},{dblOY.ToString(CultureInfo.InvariantCulture)},{dblOZ.ToString(CultureInfo.InvariantCulture)}]) ")
        '                    'stbR.AppendLine($"cylinder(h=Layer,d=Ugello);")

        '                    'stbR.Append($"translate([{dblDX.ToString(CultureInfo.InvariantCulture)},{dblDY.ToString(CultureInfo.InvariantCulture)},{dblDZ.ToString(CultureInfo.InvariantCulture)}]) ")
        '                    'stbR.AppendLine($"cylinder(h=Layer,d=Ugello);")

        '                    'stbR.AppendLine("};")

        '                End If

        '                dblOX = dblDX
        '                dblOY = dblDY
        '                dblOZ = dblDZ
        '                dblOE = dblDE
        '                dblOF = dblDF

        '        End Select

        '    End If

        'Next

        stbR.AppendLine("];")
        'stbR.AppendLine("};")

        stbR.AppendLine("")
        'stbR.AppendLine("for (xyx = gcd) drw(xyx);")
        ' stbR.AppendLine("for(i = [0 : 1 : 10000]) drw(gcd[i]);")

        stbR.AppendLine("union () {")

        Dim intS As Integer = 2000
        Dim intCnt As Integer = Segmenti.Count - 1

        For intF As Integer = 0 To intCnt Step intS
            If intF > 0 Then
                stbR.Append("//")

            End If

            stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) {"{"} echo(i + 1, {(intCnt + 1).ToString()}); drw(gcd[i]); {"}"} ")

        Next

        stbR.AppendLine("};")

        TextBoxOpenScad.Text = stbR.ToString

    End Sub

    Private Class clsProcesso
        Public Property Processo As Process = Nothing
        Public Property FileSCAD As String = String.Empty

        Public ReadOnly Property FileStl As String
            Get
                Return FileSCAD.Replace(".scad", ".stl")
            End Get
        End Property

        Private StartTime As Date = Now

        Public Enum enmStato
            Fermo
            Attivo
            Finito
            Finito_Con_Errore
            Eliminato
            Finale
        End Enum

        Public Property Stato As enmStato = enmStato.Fermo

        Public Class ProcessoEventArgs
            Inherits EventArgs
            Public Property Testo As String = String.Empty
            Public Sub New(_testo As String)
                Testo = _testo
            End Sub
        End Class

        Public Event Rapporto(sender As Object, e As ProcessoEventArgs)
        Public Event Finito(sender As Object, e As EventArgs)

        Public Sub New(_file As String)
            FileSCAD = _file

            If IO.File.Exists(FileSCAD) Then
                Processo = New Process With {
                .EnableRaisingEvents = True
            }

                Processo.StartInfo.FileName = "C:\Program Files\OpenSCAD\openscad.exe"
                Processo.StartInfo.Arguments = $"-o ""{FileStl}"" ""{FileSCAD}"""
                Processo.StartInfo.UseShellExecute = False
                Processo.StartInfo.RedirectStandardOutput = True
                Processo.StartInfo.RedirectStandardError = True

                AddHandler Processo.OutputDataReceived, Sub(ps, pe)
                                                            If pe.Data IsNot Nothing AndAlso Not String.IsNullOrEmpty(pe.Data.Trim) Then
                                                                RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > {pe.Data}"))
                                                            End If
                                                        End Sub

                AddHandler Processo.ErrorDataReceived, Sub(ps, pe)
                                                           If pe.Data IsNot Nothing AndAlso Not String.IsNullOrEmpty(pe.Data.Trim) Then
                                                               RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > {pe.Data}"))
                                                           End If
                                                       End Sub

                AddHandler Processo.Exited, Sub(ps, pe)
                                                Stato = enmStato.Finito
                                                RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > finito in {DateDiff(DateInterval.Second, StartTime, Now).ToString()} sec"))
                                                RaiseEvent Finito(Me, New EventArgs())
                                            End Sub
            Else
                Stato = enmStato.Finito_Con_Errore
                RaiseEvent Finito(Me, New EventArgs())

            End If

        End Sub

        Public Sub Avvia()
            Stato = enmStato.Attivo
            StartTime = Now

            RaiseEvent Rapporto(Me, New ProcessoEventArgs($"{FileSCAD} > Elaboro"))

            Try
                Processo.CancelOutputRead()
                Processo.CancelErrorRead()

            Catch ex As Exception
                '

            End Try

            Try
                Processo.Start()
                Processo.BeginOutputReadLine()
                Processo.BeginErrorReadLine()

            Catch ex As Exception
                '

            End Try

        End Sub

        Public Sub Uccidi()
            Stato = enmStato.Eliminato

            Try
                Processo.Kill()

            Catch ex As Exception
                ' boh

            End Try

            Processo.Dispose()

        End Sub

    End Class

    Private Processi As List(Of clsProcesso)

    Private Sub ButtonTest_Click(sender As Object, e As EventArgs) Handles ButtonTest.Click
        Modo4()
    End Sub

    Private Sub Modo1()
        Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

        If IO.Directory.Exists(strTF) Then
            IO.Directory.Delete(strTF)
        End If

        IO.Directory.CreateDirectory(strTF)

        TextBoxOpenScad.Text = strTF

        GCodeToSegmenti()

        Processi = New List(Of clsProcesso)

        Dim stbF As New StringBuilder()
        stbF.AppendLine("union () {")

        For LI As Integer = Segmenti.First.Livello To Segmenti.Last.Livello
            Dim stbR As New StringBuilder()

            stbR.AppendLine("$fn=25;")
            stbR.AppendLine("")
            stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Layer={dblUgello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine("module drw(xyz) {")
            stbR.AppendLine("        dy = abs(xyz[4] - xyz[1]);")
            stbR.AppendLine("        dx = abs(xyz[3] - xyz[0]);")
            stbR.AppendLine("")
            stbR.AppendLine("        distanza = sqrt((dy * dy) + (dx * dx));")
            stbR.AppendLine("")
            stbR.AppendLine("        if (xyz[3] >= xyz[0]) {")
            stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, atan((xyz[4] - xyz[1]) / (xyz[3] - xyz[0]))]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
            stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine("        } else {")
            stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) rotate([0, 0, (atan((xyz[1] - xyz[4]) / (xyz[0] - xyz[3])) + 180.0)]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([distanza, Ugello, Layer]);")
            stbR.AppendLine("            translate([xyz[0], xyz[1], xyz[2]]) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine("            translate([xyz[3], xyz[4], xyz[5]]) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine("     }")
            stbR.AppendLine("};")
            stbR.AppendLine("")
            stbR.Append("gcd = [")

            Dim blnPrimo As Boolean = True
            Dim intCnt As Integer = 0

            For Each sg As clsSegmento In Segmenti
                If sg.Livello = LI Then
                    intCnt += 1

                    If blnPrimo Then
                        blnPrimo = False

                    Else
                        stbR.AppendLine($",")

                    End If

                    stbR.Append($"[{sg.aStringaSemplice()}]")

                ElseIf sg.Livello > LI Then
                    Exit For

                End If

            Next

            stbR.AppendLine("];")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")

            Dim intS As Integer = 100

            intCnt -= 1

            For intF As Integer = 0 To intCnt Step intS
                stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) drw(gcd[i]); ")

            Next

            stbR.AppendLine("};")

            Dim FO As String = IO.Path.Combine(strTF, $"L{LI.ToString()}.scad")
            IO.File.WriteAllText(FO, stbR.ToString)

            Processi.Add(New clsProcesso(FO))

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Testo($"{vbCrLf}{pe.Testo}")
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)
                                                 ControllaProcessi()
                                             End Sub

            stbF.AppendLine($"import(""{Processi.Last.FileStl.Replace("\", "/")}"", convexity=5);")

        Next

        stbF.AppendLine("};")

        If True Then
            Dim FO As String = IO.Path.Combine(strTF, $"Finale.scad")
            IO.File.WriteAllText(FO, stbF.ToString)

            Processi.Add(New clsProcesso(FO))
            Processi.Last.Stato = clsProcesso.enmStato.Finale

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Testo($"{vbCrLf}{pe.Testo}")
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)
                                                 ControllaProcessi()
                                             End Sub

        End If

        ControllaProcessi()

    End Sub

    Private Sub Modo2()
        Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

        If IO.Directory.Exists(strTF) Then
            IO.Directory.Delete(strTF)
        End If

        IO.Directory.CreateDirectory(strTF)

        TextBoxOpenScad.Text = strTF

        GCodeToSegmenti()

        Processi = New List(Of clsProcesso)
        Dim ListaFile As New List(Of String)

        For LI As Integer = 0 To Segmenti.Count - 1
            Dim sg As clsSegmento = Segmenti(LI)

            Dim stbR As New StringBuilder()

            stbR.AppendLine("$fn=25;")
            stbR.AppendLine("")
            stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Layer={sg.Layer.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine($"Distanza={sg.Distanza.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Angolo={sg.Angolo.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Origine={sg.Origine.aStringaComplessa()};")
            stbR.AppendLine($"Destinazione={sg.Destinazione.aStringaComplessa()};")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")
            stbR.AppendLine($"   translate(Origine) rotate([0, 0, Angolo]) translate([0, 0-(Ugello/2), 0-(Layer/2)]) cube([Distanza, Ugello, Layer]);")
            stbR.AppendLine($"   translate(Origine) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine($"   translate(Destinazione) cylinder(d=Ugello, h=Layer,center=true);")
            stbR.AppendLine("};")

            Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")
            IO.File.WriteAllText(FO, stbR.ToString)

            Processi.Add(New clsProcesso(FO))

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Testo($"{vbCrLf}{pe.Testo}")
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)
                                                 ControllaProcessi()
                                             End Sub

            ListaFile.Add($"""{Processi.Last.FileStl.Replace("\", "/")}""")

        Next

        If True Then
            Dim blnPrimo As Boolean = True
            Dim intCnt As Integer = 0
            Dim stbR As New StringBuilder()

            stbR.Append("gcd = [")

            For Each strF As String In ListaFile

                If blnPrimo Then
                    blnPrimo = False

                Else
                    stbR.AppendLine($",")

                End If

                stbR.AppendLine(strF)

            Next

            stbR.AppendLine("];")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")

            Dim intS As Integer = 100

            intCnt = ListaFile.Count - 1

            For intF As Integer = 0 To intCnt Step intS
                stbR.AppendLine($"for(i = [{intF.ToString()} : {Math.Min(intCnt, intF + intS - 1).ToString()}]) import(gcd[i], convexity=5);")

            Next

            stbR.AppendLine("};")

            Dim FO As String = IO.Path.Combine(strTF, $"Finale.scad")
            IO.File.WriteAllText(FO, stbR.ToString)

            Processi.Add(New clsProcesso(FO))
            Processi.Last.Stato = clsProcesso.enmStato.Finale

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Testo($"{vbCrLf}{pe.Testo}")
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)
                                                 ControllaProcessi()
                                             End Sub

        End If

        ControllaProcessi()

    End Sub

    Dim blnCP As Boolean = False
    Public Sub ControllaProcessi()
        Dim tm As Date = Now.AddSeconds(30)

        Do While blnCP AndAlso Now < tm
            Threading.Thread.Sleep(1)
            My.Application.DoEvents()

        Loop

        blnCP = True

        Dim intCntProcessiAttivi As Integer = 0
        Dim intCntProcessiCompletati As Integer = 0

        For Each p As clsProcesso In Processi
            If p.Stato = clsProcesso.enmStato.Attivo Then
                intCntProcessiAttivi += 1

            ElseIf p.Stato = clsProcesso.enmStato.Finito Then
                intCntProcessiCompletati += 1

            End If

        Next

        Progresso(0, Processi.Count, intCntProcessiCompletati)

        For Each p As clsProcesso In Processi
            If intCntProcessiAttivi < 10 Then
                If p.Stato = clsProcesso.enmStato.Fermo Then
                    intCntProcessiAttivi += 1
                    p.Avvia()

                End If

            Else
                Exit For

            End If

        Next

        If intCntProcessiAttivi = 0 AndAlso Processi.Count > 0 Then
            If Processi.Last.Stato = clsProcesso.enmStato.Finale Then
                Processi.Last.Avvia()

            End If

        End If

        blnCP = False

    End Sub

    Private Sub Modo3()
        Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

        If IO.Directory.Exists(strTF) Then
            IO.Directory.Delete(strTF)
        End If

        IO.Directory.CreateDirectory(strTF)

        TextBoxOpenScad.Text = strTF

        GCodeToSegmenti()

        Processi = New List(Of clsProcesso)

        For LI As Integer = 0 To Segmenti.Count - 1
            Threading.Thread.Sleep(1)
            My.Application.DoEvents()

            Dim sg As clsSegmento = Segmenti(LI)

            Dim stbR As New StringBuilder()

            stbR.AppendLine("$fn=20;")
            stbR.AppendLine("")
            stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Layer={sg.Layer.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine($"Distanza={sg.Distanza.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Angolo={sg.Angolo.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Origine={sg.Origine.aStringaComplessa()};")
            stbR.AppendLine($"Destinazione={sg.Destinazione.aStringaComplessa()};")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")

            If Processi.Count > 0 Then
                stbR.AppendLine($"   import(""{Processi.Last.FileStl.Replace("\", "/")}"", convexity=5);")

            End If

            stbR.AppendLine($"   translate(Origine) rotate([0, 0, Angolo]) translate([0, 0-(Ugello/2), 0]) cube([Distanza, Ugello, Layer]);")
            stbR.AppendLine($"   translate(Origine) cylinder(d=Ugello, h=Layer);")
            stbR.AppendLine($"   translate(Destinazione) cylinder(d=Ugello, h=Layer);")
            stbR.AppendLine("};")

            Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")
            IO.File.WriteAllText(FO, stbR.ToString)

            Processi.Add(New clsProcesso(FO))

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Testo($"{vbCrLf}{pe.Testo}")
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)

                                                 Dim tm As Date = Now.AddSeconds(30)

                                                 Do
                                                     Threading.Thread.Sleep(1)
                                                     My.Application.DoEvents()

                                                     If IO.File.Exists(IO.Path.Combine(strTF, $"{Processi.First.FileStl}")) Then

                                                         Processi.First.Uccidi()
                                                         Processi.RemoveAt(0)

                                                         If Processi.Count > 0 Then
                                                             Processi.First.Avvia()
                                                         End If

                                                         Exit Do

                                                     End If

                                                 Loop While Now < tm

                                             End Sub

        Next

        Processi.First.Avvia()

    End Sub

    Private Sub Modo4()
        Dim strTF As String = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetTempFileName.Replace(".", ""))

        If IO.Directory.Exists(strTF) Then
            IO.Directory.Delete(strTF)
        End If

        IO.Directory.CreateDirectory(strTF)

        TextBoxOpenScad.Text = strTF

        Testo($"{vbCrLf}Calcolo i segmenti ...")

        GCodeToSegmenti()

        Processi = New List(Of clsProcesso)

        Testo($"{vbCrLf}CREO {Segmenti.Count.ToString()} FILE SCAD ...")

        Dim LC As Integer = Integer.MinValue

        For LI As Integer = 0 To Segmenti.Count - 1
            Progresso(0, Segmenti.Count, LI + 1)

            Threading.Thread.Sleep(1)
            My.Application.DoEvents()

            Dim sg As clsSegmento = Segmenti(LI)

            Dim stbR As New StringBuilder()

            stbR.AppendLine("$fn=20;")
            stbR.AppendLine("")
            stbR.AppendLine($"Ugello={dblUgello.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Layer={sg.Layer.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine("")
            stbR.AppendLine($"Distanza={sg.Distanza.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Angolo={sg.Angolo.ToString(CultureInfo.InvariantCulture)};")
            stbR.AppendLine($"Origine={sg.Origine.aStringaComplessa()};")
            stbR.AppendLine($"Destinazione={sg.Destinazione.aStringaComplessa()};")
            stbR.AppendLine("")
            stbR.AppendLine("union () {")
            stbR.AppendLine($"   translate(Origine) rotate([0, 0, Angolo]) translate([0, 0-(Ugello/2), 0]) cube([Distanza, Ugello, Layer]);")

            If sg.Livello <> LC Then
                LC = sg.Livello
                stbR.AppendLine($"   translate(Origine) cylinder(d=Ugello, h=Layer);")

            End If

            stbR.AppendLine($"   translate(Destinazione) cylinder(d=Ugello, h=Layer);")
            stbR.AppendLine("};")

            Dim FO As String = IO.Path.Combine(strTF, $"S{LI.ToString()}.scad")
            IO.File.WriteAllText(FO, stbR.ToString)

            Processi.Add(New clsProcesso(FO))

            AddHandler Processi.Last.Rapporto, Sub(ps, pe)
                                                   Log(pe.Testo)
                                               End Sub

            AddHandler Processi.Last.Finito, Sub(ps, pe)

                                                 Dim p As clsProcesso = CType(ps, clsProcesso)

                                                 If IO.File.Exists(p.FileStl) Then
                                                     IO.File.Delete(p.FileSCAD)

                                                 Else
                                                     Testo($"{p.FileStl} > Mancante")
                                                     'p.Stato = clsProcesso.enmStato.Fermo

                                                 End If

                                             End Sub

        Next

        Testo($"{vbCrLf}CREO {Segmenti.Count.ToString()} FILE STL")

        TimerControllo.Start()

    End Sub

    Delegate Sub Testo_Invoker(_Testo As String)
    Public Sub Testo(_Testo As String)
        If InvokeRequired Then
            Try
                Invoke(New Testo_Invoker(AddressOf Testo), {_Testo})

            Catch ex As Exception
                ' :-(

            End Try

        ElseIf Not String.IsNullOrEmpty(_Testo.Trim) Then
            If TextBoxOpenScad.TextLength > TextBoxOpenScad.MaxLength - (_Testo.Length * 2) Then
                TextBoxOpenScad.Clear()

            End If

            TextBoxOpenScad.SelectionStart = TextBoxOpenScad.Text.Length
            TextBoxOpenScad.SelectionLength = 0
            TextBoxOpenScad.SelectedText = _Testo

            TextBoxOpenScad.ScrollToCaret()

        End If

    End Sub

    Delegate Sub Log_Invoker(_Testo As String)
    Public Sub Log(_Testo As String)
        If InvokeRequired Then
            Try
                Invoke(New Log_Invoker(AddressOf Log), {_Testo})

            Catch ex As Exception
                ' :-(

            End Try

        ElseIf Not String.IsNullOrEmpty(_Testo.Trim) Then
            LabelLog.Text = _Testo

        End If

    End Sub


    Delegate Sub Progresso_Invoker(_min As Integer, _max As Integer, _value As Integer)
    Public Sub Progresso(_min As Integer, _max As Integer, _value As Integer)
        If InvokeRequired Then
            Try
                Invoke(New Progresso_Invoker(AddressOf Progresso), {_min, _max, _value})

            Catch ex As Exception
                ' :-(

            End Try

        Else
            ProgressBarStato.Minimum = Math.Min(_min, _max)
            ProgressBarStato.Maximum = Math.Max(_min, _max)
            ProgressBarStato.Value = Math.Min(_value, _max)

        End If

    End Sub

    'Private Sub prcX_OutputDataReceived(sender As Object, e As DataReceivedEventArgs) Handles prcOpenSCAD.OutputDataReceived
    '    Testo($"{vbCrLf}{e.Data}")
    'End Sub

    'Private Sub prcX_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs) Handles prcOpenSCAD.ErrorDataReceived
    '    Testo($"{vbCrLf}{e.Data}")
    'End Sub

    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'If prcOpenSCAD IsNot Nothing Then
        '    prcOpenSCAD.Close()
        '    '  prcOpenSCAD.Dispose()

        'End If
        If Processi IsNot Nothing Then
            For Each p As clsProcesso In Processi
                If p.Stato = clsProcesso.enmStato.Attivo Then
                    p.Uccidi()

                End If

            Next

        End If

    End Sub

    Private Sub TimerControllo_Tick(sender As Object, e As EventArgs) Handles TimerControllo.Tick
        Dim intCntProcessiAttivi As Integer = 0
        Dim intCntProcessiCompletati As Integer = 0

        For Each p As clsProcesso In Processi
            If p.Stato = clsProcesso.enmStato.Attivo Then
                intCntProcessiAttivi += 1

            ElseIf p.Stato = clsProcesso.enmStato.Finito Then
                intCntProcessiCompletati += 1

            End If

        Next

        Progresso(0, Processi.Count, intCntProcessiCompletati)

        For Each p As clsProcesso In Processi
            If intCntProcessiAttivi < 10 Then
                If p.Stato = clsProcesso.enmStato.Fermo Then
                    intCntProcessiAttivi += 1
                    p.Avvia()

                End If

            Else
                Exit For

            End If

        Next

        If intCntProcessiAttivi = 0 AndAlso Processi.Count > 0 Then
            TimerControllo.Stop()

            Testo($"{vbCrLf}CREAZIONE FILE STL FINITA ...")

            Dim strFo As String = IO.Path.Combine(New IO.FileInfo(Processi.First.FileStl).DirectoryName, "Risultato.stl")

            Testo($"{vbCrLf}INCORPORO I FILE STL IN {strFo}")

            'Dim stbX As New StringBuilder()
            'stbX.AppendLine("solid DVD")

            IO.File.AppendAllText(strFo, $"solid DVD{vbCrLf}")

            For intI As Integer = 0 To Processi.Count - 1
                Progresso(0, Processi.Count, intI + 1)

                Threading.Thread.Sleep(1)
                My.Application.DoEvents()

                Dim p As clsProcesso = Processi(intI)

                If IO.File.Exists(p.FileStl) Then
                    Log($"Incorporo {p.FileStl}")

                    Dim lstS As New List(Of String)(IO.File.ReadAllLines(p.FileStl))
                    lstS.RemoveAll(Function(x) x.Trim.ToUpper.StartsWith("SOLID") OrElse x.Trim.ToUpper.StartsWith("ENDSOLID"))

                    IO.File.AppendAllLines(strFo, lstS)

                    'Dim strS() As String = IO.File.ReadAllLines(p.FileStl)

                    'For intZ As Integer = 0 To strS.Count - 1
                    '    If strS(intZ).Trim.ToUpper.StartsWith("SOLID") Then
                    '        strS(intZ) = String.Empty
                    '        Exit For

                    '    End If

                    'Next

                    'For intZ As Integer = strS.Count - 1 To 0 Step -1
                    '    If strS(intZ).Trim.ToUpper.StartsWith("ENDSOLID") Then
                    '        strS(intZ) = String.Empty
                    '        Exit For

                    '    End If

                    'Next

                    'For Each strZ As String In strS
                    '    If Not String.IsNullOrEmpty(strZ) Then
                    '        'stbX.AppendLine(strZ)
                    '        IO.File.AppendAllText(strFo, $"{strZ}{vbCrLf}")

                    '    End If

                    'Next

                    IO.File.Delete(p.FileStl)
                Else
                    Testo($"{p.FileStl} > Non incorporato, assente!")

                End If

            Next

            IO.File.AppendAllText(strFo, $"endsolid DVD{vbCrLf}")
            'stbX.AppendLine("endsolid DVD")

            'IO.File.WriteAllText(Processi.First.FileStl, stbX.ToString)

            Testo($"{vbCrLf}FINITO !!")
            Log($"FINITO !!")

        End If
    End Sub

    'Private Sub prcOpenSCAD_Exited(sender As Object, e As EventArgs) Handles prcOpenSCAD.Exited
    '    Testo($"{vbCrLf}{FileScad.First} > Finito!")

    '    FileScad.RemoveAt(0)

    '    If FileScad.Count > 0 Then
    '        Testo($"{vbCrLf}{FileScad.First} > Elaborazione ...")

    '        prcOpenSCAD.Close()
    '        prcOpenSCAD.StartInfo.Arguments = $"-o ""{FileScad.First}.stl"" ""{FileScad.First}"""
    '        prcOpenSCAD.Start()
    '        prcOpenSCAD.BeginOutputReadLine()
    '        prcOpenSCAD.BeginErrorReadLine()

    '    Else
    '        prcOpenSCAD.Dispose()

    '    End If

    'End Sub

End Class
