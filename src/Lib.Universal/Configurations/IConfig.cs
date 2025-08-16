namespace Lib.Universal.Configurations;

public interface IConfig
{
    string this[string key] { get; set; }
}
