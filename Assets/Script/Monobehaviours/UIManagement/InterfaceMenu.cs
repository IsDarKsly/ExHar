using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InterfaceMenu : NetworkBehaviour
{

    public void PauseButton() 
    {
        MenuManager.Instance.PauseGame();
    }

    public void PartyButton() 
    {
        MenuManager.Instance.PartyMenuObj.SetActive(true);

    }

    public void InventoryButton() 
    {
        NetworkManager.Singleton.StartClient();
        MenuManager.Instance.InventoryMenuObj.SetActive(true);
    }

    public void TestBattle() 
    {
        var list = new List<Enemy>();
        list.Add(new EnemyFox());
        BattleManager.Instance.StartMatch(TRANSITIONTYPE.Fade, 1.0f, list);
    }

    public void AddOneClone() 
    {
        int[] stats = { 1, 1, 1, 1, 1};
        DataManager.Instance.AddToRoster(new Humanoid("Clone", true, RACE.Westerner, CLASS.WARRIOR, new Appearance(), stats));
    }

    public void AddEquipment() 
    {
        Armor chestPiece = new Armor();
        chestPiece.Name = "Chest";
        chestPiece.Name = "Chest Desc";
        chestPiece.equipmentslot = EQUIPMENTSLOT.Chest;
        chestPiece.EquipmentValue[DamageType.Physical] = 10;
        chestPiece.StatMultipliers[STATS.Constitution] = 1f;
        Armor headPiece = new Armor();
        headPiece.Name = "Helm";
        headPiece.Name = "Helm Desc";
        headPiece.equipmentslot = EQUIPMENTSLOT.Helm;
        headPiece.EquipmentValue[DamageType.Physical] = 10;
        headPiece.StatMultipliers[STATS.Constitution] = 1f;
        DataManager.Instance.AddToInventory(chestPiece);
        DataManager.Instance.AddToInventory(headPiece);
    }

    /// <summary>
    /// This should begin hosting
    /// </summary>
    public void StartHosting() 
    {
        NetworkManager.Singleton.StartHost();
        HostCheck();
    }

    /// <summary>
    /// This should stop hosting
    /// </summary>
    public void StopConnection() 
    {
        NetworkManager.Singleton.Shutdown();
        HostCheck();
    }

    /// <summary>
    /// Begins client connection
    /// </summary>
    public void StartClient() 
    {
        NetworkManager.Singleton.StartClient();
        HostCheck();
    }

    /// <summary>
    /// Ping client should really only work if this person pressing it is the host
    /// </summary>
    public void PingClient() 
    {
        if (NetworkManager.Singleton.IsHost) 
        {
            pingServerRpc(0, NetworkObjectId);
        }
    }

    public void SendCharacter() 
    {
        recieveCharacterRpc(Stringifier.Stringify(DataManager.Instance.playerCharacter));
    }

    /// <summary>
    /// Ping client should really only work if this person pressing it is the client
    /// </summary>
    public void PingFromClient()
    {
        if (NetworkManager.Singleton.IsClient) 
        {
            pingServerRpc(0, NetworkObjectId);
        }
    }

    /// <summary>
    /// PingServerRpc is a function that should only be run on the server
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sourceNetworkObjectId"></param>
    [Rpc(SendTo.Server)]
    public void pingServerRpc(int value, ulong sourceNetworkObjectId) 
    {
        Debug.Log($"value: {value}, This is the server being pinged by a client: {sourceNetworkObjectId}");
        value++;
        if (value < 10) 
        {
            pingClientRpc(value, NetworkObjectId);
        }
    }

    /// <summary>
    /// PingServerRpc is a function that should only be run on the server
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sourceNetworkObjectId"></param>
    [Rpc(SendTo.Server)]
    public void recieveCharacterRpc(string CharacterData)
    {
        var person = Stringifier.Destringify<Humanoid>(CharacterData);
        Debug.Log($"The server has recieved a character: {person.Name}, {person.race}, {person.gender}, {person.spec}");
    }


    [Rpc(SendTo.ClientsAndHost)]
    public void pingClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"value: {value}, This is the client being pinged by the server: {sourceNetworkObjectId}");
        value++;
        if (value < 10)
        {
            pingServerRpc(value, NetworkObjectId);
        }
    }

    public void HostCheck() 
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("You are the host!");
            // Enable host-specific UI or options
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("You are a client!");
            // Enable client-specific UI or options
        }
    }


}
