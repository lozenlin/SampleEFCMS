"C:\Program Files\Microsoft SQL Server\110\Tools\Binn\sqlcmd" -S .\sqlexpress2012 -E -i BakcupSampleEFCMS_Full.sql -o D:\SqlDataBackup\SQLEXPRESS2012\SampleEFCMS\result_Full.txt
rem §R°£ÂÂ³Æ¥÷ÀÉ
forfiles /p D:\SqlDataBackup\SQLEXPRESS2012\SampleEFCMS /m SampleEFCMS*.bak /d -3 /c "cmd /c del @path"