using System;
using UnityEngine;

namespace MarlthonShips
{
    public class ShipContainer : Container
    {
        public void ShipContainerAwake()
        {
            m_nview = (m_rootObjectOverride ? m_rootObjectOverride.GetComponent<ZNetView>() : base.GetComponent<ZNetView>());
            if(m_nview.GetZDO() == null)
            {
                return;
            }
            m_inventory = new Inventory(m_name, m_bkg, m_width, m_height);
            Inventory inventory = m_inventory;
            inventory.m_onChanged = (Action)Delegate.Combine(inventory.m_onChanged, new Action(OnContainerChanged));
            m_piece = base.GetComponent<Piece>();
            if(m_nview)
            {
                m_nview.Register($"RequestOpen {m_name}", new Action<long, long>(ShipContainerRPC_RequestOpen));
                m_nview.Register($"OpenRespons {m_name}", new Action<long, bool>(RPC_OpenRespons));
                m_nview.Register($"RequestTakeAll {m_name}", new Action<long, long>(ShipContainerRPC_RequestTakeAll));
                m_nview.Register($"TakeAllRespons {m_name}", new Action<long, bool>(RPC_TakeAllRespons));
            }
            WearNTear wearNTear = m_rootObjectOverride ? m_rootObjectOverride.GetComponent<WearNTear>() : base.GetComponent<WearNTear>();
            if(wearNTear)
            {
                WearNTear wearNTear2 = wearNTear;
                wearNTear2.m_onDestroyed = (Action)Delegate.Combine(wearNTear2.m_onDestroyed, new Action(OnDestroyed));
            }
            Destructible destructible = m_rootObjectOverride ? m_rootObjectOverride.GetComponent<Destructible>() : base.GetComponent<Destructible>();
            if(destructible)
            {
                Destructible destructible2 = destructible;
                destructible2.m_onDestroyed = (Action)Delegate.Combine(destructible2.m_onDestroyed, new Action(OnDestroyed));
            }
            if(m_nview.IsOwner() && !m_nview.GetZDO().GetBool($"addedDefaultItems {m_name}", false))
            {
                AddDefaultItems();
                m_nview.GetZDO().Set($"addedDefaultItems {m_name}", true);
            }
            base.InvokeRepeating("CheckForChanges", 0f, 1f);
        }

        public void ShipContainerUpdateUseVisual()
        {
            bool flag;
            if(m_nview.IsOwner())
            {
                flag = m_inUse;
                m_nview.GetZDO().Set($"InUse {m_name}", m_inUse ? 1 : 0);
            }
            else
            {
                flag = (m_nview.GetZDO().GetInt($"InUse {m_name}", 0) == 1);
            }
            if(m_open)
            {
                m_open.SetActive(flag);
            }
            if(m_closed)
            {
                m_closed.SetActive(!flag);
            }
        }

        public bool ShipContainerInteract(Humanoid character, bool hold, bool alt)
        {
            if(hold)
            {
                return false;
            }
            if(m_checkGuardStone && !PrivateArea.CheckAccess(base.transform.position, 0f, true, false))
            {
                return true;
            }
            long playerID = Game.instance.GetPlayerProfile().GetPlayerID();
            if(!CheckAccess(playerID))
            {
                character.Message(MessageHud.MessageType.Center, "$msg_cantopen", 0, null);
                return true;
            }
            m_nview.InvokeRPC($"RequestOpen {m_name}", new object[]
            {
            playerID
            });
            return true;
        }

        public void ShipContainerRPC_RequestOpen(long uid, long playerID)
        {
            ZLog.Log(string.Concat(new string[]
            {
            "Player ",
            uid.ToString(),
            " wants to open ",
            base.gameObject.name,
            "   im: ",
            ZDOMan.instance.GetMyID().ToString()
            }));
            if(!m_nview.IsOwner())
            {
                ZLog.Log("  but im not the owner");
                return;
            }
            if((IsInUse() || (m_wagon && m_wagon.InUse())) && uid != ZNet.instance.GetUID())
            {
                ZLog.Log("  in use");
                m_nview.InvokeRPC(uid, "OpenRespons", new object[]
                {
                false
                });
                return;
            }
            if(!CheckAccess(playerID))
            {
                ZLog.Log("  not yours");
                m_nview.InvokeRPC(uid, $"OpenRespons {m_name}", new object[]
                {
                false
                });
                return;
            }
            ZDOMan.instance.ForceSendZDO(uid, m_nview.GetZDO().m_uid);
            m_nview.GetZDO().SetOwner(uid);
            m_nview.InvokeRPC(uid, $"OpenRespons {m_name}", new object[]
            {
            true
            });
        }

        public bool ShipContainerTakeAll(Humanoid character)
        {
            if(m_checkGuardStone && !PrivateArea.CheckAccess(base.transform.position, 0f, true, false))
            {
                return false;
            }
            long playerID = Game.instance.GetPlayerProfile().GetPlayerID();
            if(!CheckAccess(playerID))
            {
                character.Message(MessageHud.MessageType.Center, "$msg_cantopen", 0, null);
                return false;
            }
            m_nview.InvokeRPC($"RequestTakeAll {m_name}", new object[]
            {
            playerID
            });
            return true;
        }

        public void ShipContainerRPC_RequestTakeAll(long uid, long playerID)
        {
            ZLog.Log(string.Concat(new string[]
            {
            "Player ",
            uid.ToString(),
            " wants to takeall from ",
            base.gameObject.name,
            "   im: ",
            ZDOMan.instance.GetMyID().ToString()
            }));
            if(!m_nview.IsOwner())
            {
                ZLog.Log("  but im not the owner");
                return;
            }
            if((IsInUse() || (m_wagon && m_wagon.InUse())) && uid != ZNet.instance.GetUID())
            {
                ZLog.Log("  in use");
                m_nview.InvokeRPC(uid, $"TakeAllRespons {m_name}", new object[]
                {
                false
                });
                return;
            }
            if(!CheckAccess(playerID))
            {
                ZLog.Log("  not yours");
                m_nview.InvokeRPC(uid, $"TakeAllRespons {m_name}", new object[]
                {
                false
                });
                return;
            }
            if(Time.time - m_lastTakeAllTime < 2f)
            {
                return;
            }
            m_lastTakeAllTime = Time.time;
            m_nview.InvokeRPC(uid, $"TakeAllRespons {m_name}", new object[]
            {
            true
            });
        }

        public void ShipContainerSave()
        {
            ZPackage zpackage = new();
            m_inventory.Save(zpackage);
            string @base = zpackage.GetBase64();
            m_nview.GetZDO().Set($"items {m_name}", @base);
            m_lastRevision = m_nview.GetZDO().m_dataRevision;
            m_lastDataString = @base;
        }

        public void ShipContainerLoad()
        {
            if(m_nview.GetZDO().m_dataRevision == m_lastRevision)
            {
                return;
            }
            string @string = m_nview.GetZDO().GetString($"items {m_name}", "");
            if(string.IsNullOrEmpty(@string) || @string == m_lastDataString)
            {
                return;
            }
            ZPackage pkg = new(@string);
            m_loading = true;
            m_inventory.Load(pkg);
            m_loading = false;
            m_lastRevision = m_nview.GetZDO().m_dataRevision;
            m_lastDataString = @string;
        }

    }
}