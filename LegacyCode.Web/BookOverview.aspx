<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookOverview.aspx.cs" Inherits="LegacyCode.BookOverview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView runat="server" ID="gridView" />
        <asp:Label runat="server" ID="errorLabel" />
    </div>
    </form>
</body>
</html>
