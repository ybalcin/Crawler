# Crawler

## Getting Started

If this is your first time encountering .NET, please follow [the instructions](https://dotnet.microsoft.com/en-us/download) to
install .NET on your computer.

## Usage

### Crawler
```shell
git clone https://github.com/ybalcin/Crawler.git

cd Crawler.App/

dotnet run
```

### API
```shell
cd Crawler.API/

dotnet run
```

After `dotnet run` API is running on `https://localhost:5003`, and swagger address is `https://localhost:5003/swagger/index.html`
You can browse and see crawled data from swagger.

## Connecting to DB

[Download Studio3T](https://studio3t.com/download/)

Click Connect > NewConnection, after that copy MongoURI from appsettings.json and paste to Connection URI field. 
Then click Next and connect.