Server code for the updated version of pedalpricer.com
---
#### <em>Does this app really need a separate backend server?</em>
Not really. I had one in the old version so I could learn full-stack development, and I figured I'd keep that pattern. Back then I had no idea what I was doing in C#, so this is more an excuse to improve that code.

As of 5-26-25 I've switched from SQL Server to Sqlite. The database file is provided, data courtesy of [pedalplayground](https://github.com/PedalPlayground/pedalplayground).

All items follow the same schema:
```json
[
    {
        "id": "15ca6508-19d5-43ff-24ce-08dd8226a971",
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

To run the server you'll need an S3 bucket which contains all the image files in separate folders (pedals, pedalboards, and powersupplies). Then set the following four values using `dotnet user-secrets set "{key}" "{value}"`:
<ul>
  <li>AWS:BucketName</li>
  <li>AWS:Region</li>
  <li>AWS:AccessKey</li>
  <li>AWS:SecretKey</li>
</ul>
