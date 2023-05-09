这是一个简单的二次封装调用DotNetty进行TCP通信的案例,基于.NET4.7.2版本. 也兼容Unity与.NET Core.
在网络消息收发时对粘包进行了处理. 您可以在该范例的基础上进行个性化的研发.
NetworkHelper引用了DotNetty.Transport/Newtonsoft.Json包
NetworkSocket引用了DotNetty.Handlers/DotNetty.Transport包.
DotNetty_SocketCommunication引用了DotNetty.Transport.Libuv/Newtonsoft.Json包.
需要注意的是: 在引用DotNetty.Transport.Libuv后程序不会在构建时自动将Libuv生成在目录下,如果您发生了未找到DLL文件或加载文件格式异常错误,请将DotNetty_SocketCommunication/Libuv1.10.0DLL文件夹下找到所需要使用的dll文件放在运行目录下. 您也可以在项目的packages文件夹中找到这些文件.

This is a simple example of a second-level encapsulation of TCP communication using DotNetty, based on the .NET 4.7.2 version. It is also compatible with Unity and .NET Core. The example handles the issue of packet fragmentation during network message transmission and reception. You can customize your development based on this example.

NetworkHelper references the DotNetty.Transport and Newtonsoft.Json packages, while NetworkSocket references the DotNetty.Handlers and DotNetty.Transport packages. Simple_NettySocket references the DotNetty.Transport.Libuv and Newtonsoft.Json packages.

Note that after referencing DotNetty.Transport.Libuv, the program will not automatically generate Libuv in the directory during the build. If you encounter a DLL file not found error, please place the required DLL files in the running directory under the Simple_NettySocket/Libuv1.10.0DLL folder. You can also find these files in the packages folder of your project.
# DotNetty_SocketCommunication
