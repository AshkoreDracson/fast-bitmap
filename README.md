#How to use

###New instance
```cs
FastBitmap f = new FastBitmap(width, height);
```

###Load from file (JPG, BMP)
```cs
FastBitmap f = FastBitmap.FromFile(path);
```

###Save to file (JPG, BMP)
```cs
FastBitmap f = new FastBitmap(width, height);
// Do some stuff
f.Save(path);
```

###Bitmap to FastBitmap
```cs
Bitmap b = new Bitmap();
FastBitmap f = b;
```

###FastBitmap to Bitmap
```cs
FastBitmap f = new FastBitmap(width, height);
// Do some stuff
Bitmap b = f;
```

#Coming soon

  - PNG support + 32bbp + 16bbp + 8bbp support
  - More methods to edit images (Effects, etc...)
