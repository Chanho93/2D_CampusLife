using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    [System.Serializable]   //? 컴퓨터에서 읽고 쓰기 쉽게 설정
    public class Data
    {
        //? 세이브할 로직에 대한 속성
        public float playerX;   //? 플레이어 X 좌표
        public float playerY;   //? 플레이어 Y 좌표

        public int playerHp;
     
        public int playerCurrentHP;

/*
        /// <summary>
        /// 저장하기
        /// </summary>
        public void CallSave()
        {
            //? 2진 파일 생성
            BinaryFormatter bf = new BinaryFormatter();
           // FileStream file = File.Create(Application.dataPath + "\\SaveFile.dat");

            Data data = null;
            bf.Serialize(file, data);
            file.Close();

            Debug.Log(Application.dataPath + "의 위치에 저장했습니다.");
        }

        /// <summary>
        /// 불러오기
        /// </summary>
        public void CallLoad()
        {
            //? 2진 파일 생성
            BinaryFormatter bf = new BinaryFormatter();
           //FileStream file = File.Open(Application.dataPath + "\\SaveFile.dat");

            if(file != null && file.Length > 0)
            {
                //? 파일이 있는 경우 나 내용이 있는 경우 로드 실행
                Data data = (Data)bf.Deserialize(file);     //? 직렬화에서 원래대로 바꾼다.


            }

            Debug.Log(Application.dataPath + "의 위치에 저장했습니다.");
        }

    */

    }
}
