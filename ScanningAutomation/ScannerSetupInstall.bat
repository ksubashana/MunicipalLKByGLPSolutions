@echo off
echo ======================================
echo 🏛️  Municipal Scanner Setup Starting...
echo ======================================

echo 🧾 Installing NAPS2 silently...
start /wait NAPS2-Setup.exe /S

echo 📁 Creating scan folder at C:\MunicipalUploads...
mkdir "C:\MunicipalUploads"

echo 📂 Importing scanning profile...
start /wait NAPS2.Console.exe -i "MunicipalScan.xml"

echo 🖨️ Placing Scan Document shortcut on desktop...
copy scan.bat "%PUBLIC%\Desktop\Scan Document.bat"

echo ✅ Setup complete. You can now scan documents and drag into the system.
pause
