using UnityEngine;

namespace MarlthonShips
{
    public class ShipOars : MonoBehaviour
    {
        private void Start()
        {
            oarsAnimator = Utils.FindChild(transform, "Remo").GetComponent<Animator>();
            if(!oarsAnimator)
            {
                Debug.Log("there is no link to the animator of oars (não há ligação com o animador de remos)");
            }
            if(!ship)
            {
                Debug.Log("there is no link to the ship of oars (não há ligação com o navio de remos)");
            }
        }

        public void FixedUpdate()
        {
            if(!oarsAnimator) return;
            if(!ship) return;
            if(!Player.m_localPlayer) return;

            if(Player.m_localPlayer.GetControlledShip() && ship.GetSpeed() > 0.1f)
                oarsAnimator.StopPlayback();
            else
                oarsAnimator.StartPlayback();
        }

        public Animator oarsAnimator;
        public Ship ship;
    }
}