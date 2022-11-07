Imports System.Data.OleDb
Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.ComponentModel
Imports System.Reflection
Public Class Form1
    Dim lv As ListViewItem
    Dim selected As String

    Private Sub PopListView()
        ListView1.Clear()

        With ListView1
            .View = View.Details
            .GridLines = True
            .Columns.Add("No", 40)
            .Columns.Add("Lastname", 100)
            .Columns.Add("Firstname", 100)
            .Columns.Add("Mi", 40)
            .Columns.Add("Gender", 60)
            .Columns.Add("Department", 95)
            .Columns.Add("Contact", 95)
        End With

        openCon()
        sql = "Select * from tbl_teachersInfo order by teacherID"
        cmd = New OleDbCommand(sql, cn)
        dr = cmd.ExecuteReader()

        Do While dr.Read() = True
            lv = New ListViewItem(dr("teacherID").ToString)
            lv.SubItems.Add(dr("lastname"))
            lv.SubItems.Add(dr("firstname"))
            lv.SubItems.Add(dr("middleinitial"))
            lv.SubItems.Add(dr("gender"))
            lv.SubItems.Add(dr("department"))
            lv.SubItems.Add(dr("contact"))
            ListView1.Items.Add(lv)
        Loop
        dr.Close()
        cn.Close()
    End Sub

    Public Sub reset()
        txtno.Text = ""
        txtlastname.Text = ""
        txtfirstname.Text = ""
        txtmi.Text = ""
        cmbgender.Text = Nothing
        cmbdepartment.Text = Nothing
        txtaddress.Text = ""
        txtcontact.Text = ""

        txtlastname.Enabled = False
        txtlastname.Enabled = False
        txtmi.Enabled = False
        cmbgender.Enabled = False
        cmbdepartment.Enabled = False
        txtaddress.Enabled = False
        txtcontact.Enabled = False

        btnClose.Text = "Close"
        btnNew.Text = "New"
        btnNew.ForeColor = Color.Blue
        btnNew.Enabled = True
        btnSave.Enabled = False
        btnDelete.Enabled = False
        btnSave.Enabled = False

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If btnNew.Text = "Edit" Then
            btnSave.Text = "Update"
            btnNew.Enabled = True
        Else
            btnSave.Text = "Save"
            btnDelete.Enabled = False
        End If

        Me.txtlastname.Enabled = True
        Me.txtlastname.Enabled = True
        Me.txtmi.Enabled = True
        Me.cmbgender.Enabled = True
        Me.cmbdepartment.Enabled = True
        Me.txtaddress.Enabled = True
        Me.txtcontact.Enabled = True

        Me.btnClose.Text = "Cancel"
        btnClose.ForeColor = Color.Red
        btnNew.Enabled = False
        btnSave.Enabled = True

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtlastname.Text = "" Or txtfirstname.Text = "" Or txtmi.Text = "" Or cmbgender.Text = "" Or cmbdepartment.Text = "" Or txtaddress.Text = "" Or txtcontact.Text = "" Then
            MsgBox("Please complete all data before saving.", 48, Me.Text)
        Else
            If btnSave.Text = "Save" Then
                If MsgBox("Are you sure you want to add this on the record?", vbQuestion + vbYesNo) = vbYes Then
                    openCon()
                    sql = "INSERT INTO tbl_teachersInfo (teacherID, lastname, firstname, middleinitial, gender, department, address, contact) VALUES('" & Me.txtno.Text & "','" & Me.txtlastname.Text & "','" & Me.txtfirstname.Text & "','" & Me.txtmi.Text & "','" & Me.cmbgender.Text & "','" & Me.cmbdepartment.Text & "','" & Me.txtaddress.Text & "','" & Me.txtcontact.Text & "')"
                    cmd = New OleDbCommand(sql, cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                    MsgBox("Record Saved!", 64, Me.Text)
                    PopListView()
                    reset()
                End If
            Else
                If MsgBox("Are you sure you want to update this on the record?", vbQuestion + vbYesNo) = vbYes Then
                    openCon()
                    sql = "UPDATE tbl_teachersInfo SET lastname = '" & Me.txtlastname.Text & "',firstname = '" & Me.txtfirstname.Text & "',middleinitial = '" & Me.txtmi.Text & "',gender='" & Me.cmbgender.Text & "',department = '" & Me.cmbdepartment.Text & "',address = '" & Me.txtaddress.Text & "',contact = '" & Me.txtcontact.Text & "' WHERE teacherID =" & selected
                    cmd = New OleDbCommand(sql, cn)
                    cmd.ExecuteNonQuery()
                    cn.Close()
                    MsgBox("Record Saved!", 64, Me.Text)
                    PopListView()
                    reset()
                End If
            End If
        End If

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If btnClose.Text = "Cancel" Then
            reset()
        Else
            Me.Close()
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopListView()

    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Dim i As Integer
        For i = 0 To ListView1.SelectedItems.Count - 1

            selected = ListView1.SelectedItems(i).Text
            openCon()
            sql = "Select * from tbl_teachersInfo where teacherID = " & selected
            cmd = New OleDbCommand(sql, cn)
            dr = cmd.ExecuteReader()

            dr.Read()

            Me.txtno.Text = dr("teacherID")
            Me.txtlastname.Text = dr("lastname")
            Me.txtfirstname.Text = dr("firstname")
            Me.txtmi.Text = dr("middleinitial")
            Me.cmbgender.Text = dr("gender")
            Me.cmbdepartment.Text = dr("department")
            Me.txtaddress.Text = dr("address")
            Me.txtcontact.Text = dr("contact")

            dr.Close()
            cn.Close()
        Next

        btnClose.Text = "Cancel"
        btnClose.ForeColor = Color.Red
        btnNew.Text = "Edit"
        btnNew.ForeColor = Color.Blue
        btnNew.Enabled = True
        btnSave.Enabled = False
        btnDelete.Enabled = True

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If MsgBox("Are you sure to delete this record?", vbQuestion + vbYesNo) = vbYes Then
            openCon()
            sql = "DELETE FROM tbl_teachersInfo where teacherID =" & selected
            cmd = New OleDbCommand(sql, cn)
            cmd.ExecuteNonQuery()
            cn.Close()
            MsgBox("Record Deleted!", 64, Me.Text)

            PopListView()
            reset()

        End If

    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles txtlastname.TextChanged, txtfirstname.TextChanged, txtmi.TextChanged, cmbgender.TextChanged, cmbdepartment.TextChanged, txtaddress.TextChanged, txtcontact.TextChanged
        If txtlastname.Text = "" And txtfirstname.Text = "" And txtmi.Text = "" And cmbgender.Text = "" And cmbdepartment.Text = "" And txtaddress.Text = "" And txtcontact.Text = "" Then
            btnDelete.Enabled = False
        Else
            btnDelete.Enabled = True
        End If

    End Sub
End Class
