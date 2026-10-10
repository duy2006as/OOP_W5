using System;

public class Printer : Device
{
    public string Type { get; set; } // "Laser" hoặc "Phun"
    public int PrintedPages { get; set; }
    public bool IsColor { get; set; }

    public Printer(string id, string name, int year, decimal price, DeviceStatus status, 
                   string type, int printedPages, bool isColor) 
        : base(id, name, year, price, status)
    {
        Type = type;
        PrintedPages = printedPages;
        IsColor = isColor;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = Price * 0.04m;
        if (PrintedPages > 100000) cost += 500000m;
        if (IsColor) cost += 300000m;
        return cost;
    }
    
    public override string ToString() => base.ToString() + $" - Máy in ({Type}, Màu: {IsColor}, Trang in: {PrintedPages})";
}

public class NetworkPrinter : Printer, INetworkable
{
    public string IpAddress { get; private set; }
    public bool IsConnected { get; private set; }

    public NetworkPrinter(string id, string name, int year, decimal price, DeviceStatus status, 
                          string type, int printedPages, bool isColor) 
        : base(id, name, year, price, status, type, printedPages, isColor)
    {
    }

    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("IP không được rỗng.");
        if (IsConnected)
            throw new InvalidOperationException("Thiết bị đang kết nối.");
        
        IpAddress = ipAddress;
        IsConnected = true;
    }

    public void Disconnect()
    {
        IsConnected = false;
        IpAddress = null;
    }

    public override string ToString() => base.ToString() + $" - (Mạng: {(IsConnected ? IpAddress : "Không kết nối")})";
}