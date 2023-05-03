using System.Security.Cryptography;
using System.Text;

namespace AA_L3;

public class Blockchain
{
    private byte[] _prev;
    private readonly float _delta;

    public Blockchain(float delta)
    {
        _delta = delta;
        _prev = Array.Empty<byte>();
    }
 
    public bool CreateNewBlock()
    {
        Random random = new Random();
        var nonce =  BitConverter.GetBytes(random.Next());
        var hash = GetHash(nonce.Concat(_prev).ToArray());
        var hashFloat = BitConverter.ToDouble(hash);

        if (hashFloat > _delta)
        {
            _prev = hash;
            return true;
        }

        return false;
    }
    

    private static byte[] GetHash(byte[] inputString)
    {
        return SHA256.HashData(inputString);
    }
}