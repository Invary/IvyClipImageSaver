# IvyClipImageSaver for windows



<br />

## 📝 About
IvyClipImageSaver is a command line tool for saving image files from the clipboard.


<br />

## ⚡ Featurees

* Support command line options.
* Support png, jpg, bmp, gif and tiff.
* Support triming.
* Support resizing.

<br />

## 🔨 Command line arguments
--folder {folder path} <br/>
--filename {name} <br/>
        name can include the following, which will be replaced at runtime. <br/>
                :year:  :month:  :day:  :hour24:  :hour12:  :min:  :sec: <br/>
--format [png|jpg|bmp|tiff|gif] <br/>
--overwrite [true|false] <br/>
--cuttop {number of pixel to cut} <br/>
--cutbottom {number of pixel to cut} <br/>
--cutleft {number of pixel to cut} <br/>
--resize {width},{hight} <br/>
--beep-succeeded [on|off] <br/>
--beep-failed [on|off] <br/>
 <br/>

 * example<br/>
 This command save image file to desktop folder `20220313_161645.png`
```
  IvyClipImageSaver.exe
```
 <br/>

 This command save image file to ex. `g:/desktop\test_2022_03_13.jpg`
  ```
    IvyClipImageSaver.exe --folder "g:/desktop" --filename "test_:year:_:month:_:day:" --format jpg --overwrite true
  ```
  <br/>

  This command save image file to desktop, with the image cut and resized.
   ```
     IvyClipImageSaver.exe --cuttop 50 --cutbottom 10 --resize 800,600
   ```

<br />

## ️🛠️ Requirements
  🗸 Windows 7 or later<br/>
  🗸 .NET 6.0 runtime library<br/>

<br />

## 📥 Install
1. [Download](https://github.com/Invary/IvyClipImageSaver/releases) latest version of IvyClipImageSaver
2. Extract it to installation folder
3. Start **`IvyClipImageSaver.exe`** in the installation folder.
4. And if image exists in clipboard, save it to image file.

<br />

## 🔐 Privacy policy
- Do not send privacy information.
- Do not display online ads.
- Do not collect telemetry information.
- If online information is needed, get it from github repositories whenever possible.

<br />


## 📈 Changelog

- [Ver100](https://github.com/Invary/IvyClipImageSaver/releases/tag/Ver100)
Initial release

<br />

## 📩 Suggestions and feedback
If you have any idea or suggestion, please add a github issue.

<br />


## ⭐ Price

FREE of charge. <br />
If you would like to donate, please send me a ko-fi or crypto. It's a great encouragement!

[![ko-fi](https://raw.githubusercontent.com/Invary/IvyMediaDownloader/main/img/donation_kofi.png)](https://ko-fi.com/E1E7AC6QH)

- Address: 0xCbd4355d13CEA25D87F324E9f35A075adce6507c<br/>
 -- Binance Smart Chain (BNB, USDT, BUSD etc.)<br/>
 -- Polygon network (MATIC, USDC etc.)<br/>
 -- Ethereum network (ETH)<br/>

- Address: 1FvzxYriyNDdeA12eaUGXTGSJxkzpQdxPd<br/>
 -- Bitcoin (BTC)<br/>

<br />


<br />
<br />
<br />

#### Copyright (c) Invary
