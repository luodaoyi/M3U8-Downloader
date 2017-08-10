# M3U8-Downloader
 * **The New Version** : <https://github.com/nilaoda/The-New-M3U8-Downloader>

一个基于FFmpeg (https://github.com/FFmpeg/FFmpeg) 的M3U8下载器，仅仅服务于需要它的人！  

百度网盘分流：http://pan.baidu.com/s/1dF4uDuL

<img src="https://camo.githubusercontent.com/2ced699dc147e0d9015973139b5d82666758309a/687474703a2f2f69322e6275696d672e636f6d2f3536373537312f313331666130633630616139396661652e706e67" alt="M3U8-Downloader" />


增加了一个简易http服务器，
默认监听 http://localhost:8080
文件名参数为 file
下载地址参数为 url

如果下载某个视频 那么可以直接post过去为

http://localhost:8080?file=av.mp4&url=xxxxxx
