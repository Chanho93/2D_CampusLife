using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Linq;
using System;

namespace HJ.Manager
{
    [TypeInfoBox("Map 목록 리스트 관리 메니져 스크립트이다. 이곳에서 ")]
    public class MapManager : Singleton<MapManager>
    {
        // 모든 맵 Bound
        [Title("Setting"), LabelText("맵 영역 박스콜라이더 2D"), InfoBox("0 - 캠퍼스, 1 - 학교, 2 - 집, 3 - 피시방 \n 4 - 커피, 5 - 도서관, 6 - 술집")]
        public BoxCollider2D[] mapBoundBoxCollider;

        // 캠퍼스, 대학교, 집, 피시방, 커피숍, 도서관, 술집
        public enum MAPLISTENUM { CAMPAUS, UNIVERCITY, HOME, PCROOM, COFFEE, BOOK, BAR}

        [EnumToggleButtons]
        public MAPLISTENUM mapList;


    }
}

