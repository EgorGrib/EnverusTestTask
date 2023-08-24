Application downloads an excel file from a link (you need to specify the name of the file to download), then converts it to csv format and filters the data (leaves only the necessary years of data).

In the file appsettings.json you can specify the necessary parameters, namely the url of the website where the file is located, the name of the file, the number of recent years of data that should be left and the path where the file will be saved.

Application is covered by unit tests using nUnit.

