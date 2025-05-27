Server code for the updated version of pedalpricer.com
---
#### <em>Does this app really need a separate backend server?</em>
Not really. I had one in the old version so I could learn full-stack development, and I figured I'd keep that pattern. Back then I had no idea what I was doing in C#, so this is more an excuse to improve that code.

This branch is (almost) functionally the same as the master branch but uses Sqlite instead of SQL Server.

All items follow the same schema:
```json
[
    {
        "id": 00000000-0000-0000-0000-000000000000,
        "brand": "MXR",
        "name": "Dyna Comp",
        "width": 2.670,
        "height": 4.500,
        "filename": "mxr-m102.png"
    }
]
```

Filename points to file hosted on an S3 bucket.
Complete documentation can be found via the built-in swagger docs.

To run the server, provide a connection string in `appsettings.Development.json` and set the following four values using `dotnet user-secrets set "{key}" "{value}"`:
<ul>
  <li>AWS:BucketName</li>
  <li>AWS:Region</li>
  <li>AWS:AccessKey</li>
  <li>AWS:SecretKey</li>
</ul>
