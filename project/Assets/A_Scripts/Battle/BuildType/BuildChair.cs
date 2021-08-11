using EazyGF;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class BuildChair : BuildEquip
    {
        
        public GameObject chairEffect;
        List<BuildComModel> datas;

        public List<Transform> combuildGrid;

        public Transform buildPosGrid;

        [ContextMenu("InitBuildGrid")]
        public void InitBuildGrid()
        {
            buildPos.Clear();
            for (int i = 0; i < buildPosGrid.childCount; i++)
            {
                buildPos.Add(buildPosGrid.GetChild(i));
            }

            combuildGrid.Clear();
            for (int i = 0; i < buildPos.Count; i++)
            {
                combuildGrid.Add(buildPos[i].GetChild(0).GetChild(0));
            }
        }

        public Transform boxGrid;
        [ContextMenu("InitBoxGrid")]
        public void InitBoxGrid()
        {
            Transform grid = combuildGrid[0].transform;

            BuildEditTool.RemoveChild(boxGrid);

            for (int i = 0; i < grid.childCount; i++)
            {
                MeshRenderer mesh = grid.GetChild(i).GetComponentInChildren<MeshRenderer>();
                GameObject go = BuildEditTool.CreateBoxByPre(boxGrid, mesh.gameObject, $"{i}");
                go.gameObject.layer = 13;
            }
        }

        public override void ShowBuild()
        {
            for (int i = 0; i < buildPos.Count; i++)
            {
                buildPos[i].gameObject.SetActive(true);
            }

            List<BuildComModel> datas = BuildMgr.GetCommboById(MBuildDataModel.Id);

            for (int i = 0; i < datas.Count; i++)
            {
                ShowComBuild(datas[i].pos, datas[i].level);
            }
        }

        public override void ShowComBuild(int pos, int level)
        {
            for (int i = 0; i < combuildGrid.Count; i++)
            {
                combuildGrid[i].GetChild(pos).gameObject.SetActive(i + 1 == level);
            }
        }

        public override void PlayComEffect(int pos, int level)
        {
            chairEffect.transform.position = combuildGrid[level - 1].GetChild(pos).GetComponentInChildren<MeshRenderer>().bounds.center;

            chairEffect.gameObject.SetActive(false);
            chairEffect.gameObject.SetActive(true);

            StartCoroutine(UICommonUtil.Instance.DelayedHandle((obj) =>
            {
                chairEffect.gameObject.SetActive(false);
                MusicMgr.Instance.PlayMusicEff("d_building_fix_finish");
            }, 0.5f));
        }

        public override Vector3 GetShowBuildPos(int pos, int level)
        {
            return combuildGrid[level - 1].GetChild(pos).GetComponentInChildren<MeshRenderer>().bounds.center;
        }

        public override MeshRenderer GetShowBuildMR(int pos, int level)
        {
            return combuildGrid[level - 1].GetChild(pos).GetComponentInChildren<MeshRenderer>();
        }
    }
}
