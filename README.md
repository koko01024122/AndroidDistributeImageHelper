# AndroidDistributeImageHelper
Android国内多渠道分发Logo、宣传图转换助手


# 初衷
作者是一只Android程序员，因为国内有很多Android分发平台，每当新项目要上线的时候，总是需要去做很多个不同尺寸，不同大小的Logo,产品介绍图等，让UI做这件事情UI也不愿意做 ，确实太枯燥了，所以本着程序员就是要偷懒的精神（没有这精神），我做了这样一个工具，希望能帮助国内各位需要发包的朋友们，至于为什么作为Android 程序员我没有使用JAVA来做而使用了C#，主要是因为大部分发包的人可能是各个公司的非技术人员，用这工具还需要去装个JDK就太麻烦了，，第一个版本做的东西也不多，只有少数几个我统计过的平台需要的尺寸，后续会继续完善，也希望各位朋友们能够参与进来，共同完善这个工具，谢谢。

# 用法
下载地址： https://pan.baidu.com/s/1hsxNxju 密码: a7xk
下载下来根据工具的提示来执行就好啦


# 如何添加新规格
如果你会写代码且同时有VS的话，那就clone项目，在initLogos() (logo规则初始化) 和initAds() (宣传规则初始化)方法中，新建对应的对象，将其添加到对应的List中即可
示例
``` C#
 logoimage yybLogoimage = new logoimage("应用宝大图标.png", 512, 200, 0);//参数分别为：名称（目前需要带个.png，必须是png），尺寸，最大占用空间,圆角值
 logoimage yybminLogoimage = new logoimage("应用宝小图标.png", 16, 20, 0);
  logoList.Add(yybLogoimage);
  logoList.Add(yybminLogoimage);
```

如果你不会写代码，欢迎提交Issues,如果有新的想法也可以提交给我，我会尽快解决。


# 联系方式
邮箱：koko010@qq.com