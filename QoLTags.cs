using System.IO;
using System.Reflection;
using BepInEx;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace BananaWatch.QoLTags
{
    [BepInPlugin("com.M4.QoLTags", "QoLTags", "1.0.0")]
    public class QoLTag : BaseUnityPlugin
    {
        private static AssetBundle qoltag;
        private static GameObject qoltagprefab;
        private static bool nametagEnabled = false; 
        private static bool showPlatform = true;
        private static bool namesshown = true;

        private static bool initited;
        private void Update()
        {
            if (!initited && Player.Instance != null)
            {
                qoltag = LoadAssetBundle("BananaWatch.Resources.qoltag");
                initited = true;
            }

            if (!PhotonNetwork.InRoom) return;

            foreach (VRRig vrrgi in GorillaParent.instance.vrrigs)
            {
                if (vrrgi.isOfflineVRRig) continue;
                if (nametagEnabled)
                {
                    qoltagprefab = qoltag.LoadAsset<GameObject>("QoLTag");
                    GameObject QoLTag = Instantiate(qoltagprefab);
                    QoLTag.transform.SetParent(vrrgi.transform);
                    QoLTag.transform.localPosition = new Vector3(0f, 0.6f, 0f);
                    QoLTag.transform.localScale = Vector3.one * 0.8f;
                    Quaternion currotaitotn = QoLTag.transform.rotation;
                    Vector3 headdir = Player.Instance.headCollider.transform.position - QoLTag.transform.position;
                    float yrot = Mathf.Atan2(headdir.x, headdir.z) * Mathf.Rad2Deg;
                    QoLTag.transform.rotation = Quaternion.Euler(currotaitotn.eulerAngles.x, yrot, currotaitotn.eulerAngles.z);

                    if (namesshown)
                    {
                        Text qoltagText = QoLTag.GetComponentInChildren<Text>();
                        qoltagText.text = $"{vrrgi.OwningNetPlayer.NickName}";
                        qoltagText.color = vrrgi.mainSkin.material.color;
                    }
                    else
                    {
                        Text qoltagText = QoLTag.GetComponentInChildren<Text>();
                        qoltagText.text = " ";
                    }

                    if (showPlatform)
                    {
                        bool steamcosm = vrrgi.concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN");
                        Transform QoLCanvas = QoLTag.transform.Find("Canvas");
                        QoLCanvas.Find(steamcosm ? "PlatformSteam" : "PlatformMeta").gameObject.SetActive(true);
                        QoLCanvas.Find(steamcosm ? "PlatformMeta" : "PlatformSteam").gameObject.SetActive(false);
                    }

                    GameObject.Destroy(QoLTag, Time.deltaTime);
                }
            }
        }
        public static void ToggleNametag(bool enabled)
        {
            nametagEnabled = enabled;
        }

        public static void TogglePlatform(bool enabled)
        {
            showPlatform = enabled;
        }

        public static void ToggleNames(bool enabled)
        {
            namesshown = enabled;
        }

        private static string FormatPath(string path)
        {
            return path.Replace("/", ".").Replace("\\", ".");
        }

        public static AssetBundle LoadAssetBundle(string path)
        {
            path = FormatPath(path);
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

            if (stream == null)
            {
                return null;
            }

            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            if (bundle == null)
            {
                return null;
            }

            stream.Close();
            return bundle;
        }
    }
}
