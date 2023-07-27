using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
    // 기즈모 색상
    //public readonly static Dictionary<e_gizmo_color_type, Color> dic_gizmo_color = new Dictionary<e_gizmo_color_type, Color>
    //{
    //    {e_gizmo_color_type.WHITE,  Color.white },  {e_gizmo_color_type.BLACK, Color.black }, {e_gizmo_color_type.GRAY,    Color.gray },
    //    {e_gizmo_color_type.RED,    Color.red },    {e_gizmo_color_type.GREEN, Color.green }, {e_gizmo_color_type.BLUE,    Color.blue },
    //    {e_gizmo_color_type.YELLOW, Color.yellow }, {e_gizmo_color_type.CYAN,  Color.cyan },  {e_gizmo_color_type.MAGENTA, Color.magenta },
    //};

    // 색상
    public readonly static Color originalColor   = new Color(255, 255, 255, 255);
    public readonly static Color spriteFadeColor = new Color(255, 255, 255, 0);
    public readonly static Color iconFadeColor   = new Color(255, 255, 255, 125);

    // 방향
    public readonly static Vector3 leftUpDiagonalDir    = new Vector3(-1f, 1f);
    public readonly static Vector3 rightUpDiagonalDir   = new Vector3(1f, 1f);
    public readonly static Vector3 leftDownDiagonalDir  = new Vector3(-1f, -1f);
    public readonly static Vector3 rightDownDiagonalDir = new Vector3(1f, -1f);

    // 회전
    public readonly static Quaternion zeroRot = new Quaternion();
    public readonly static Quaternion halfRot = new Quaternion(0f,-180f,0f,0f);

    // Z축 회전
    public readonly static Vector3 leftZRot  = new Vector3(0f, 0f, 90f);
    public readonly static Vector3 downZRot  = new Vector3(0f, 0f, 180f);
    public readonly static Vector3 rightZRot = new Vector3(0f, 0f, 270f);

    public static InputComponent inputComp;

    //// ------- UI VARIABLES -------
    //public readonly static float gameBottomPos          = -16.5f;
    //public readonly static float default_power_up_ui_time = 0.05f;

    //// Player variables
    //public readonly static int power_up_item_array_index     = 5;
    //public readonly static int power_up_position_array_index = 26;

    //// 아이템 위치 관련
    //public static Vector3[] arr_power_up_pos;

    //// 값 초기화
    //public static void Init_arr_power_up_pos()
    //{
    //    // 위치 배열 업데이트
    //    arr_power_up_pos = new Vector3[power_up_position_array_index];
    //    float pos_x = -9.75f;

    //    for (int i = 0; i < power_up_position_array_index; i++)
    //    {
    //        arr_power_up_pos[i].x = pos_x;
    //        arr_power_up_pos[i].y = 25.3f;
    //        arr_power_up_pos[i].z = 3f;
    //        pos_x += 0.75f;
    //    }
    //}

    #region Random Functions

    // 랜덤형 int 변수
    public static int Rand(int _min,int _max) { return Random.Range(_min, _max); }

    // 랜덤형 float 실수
    public static float Rand(float _min, float _max) { return Random.Range(_min, _max); }

    #endregion

    #region Raycast Functions

    // 레이캐스트 레이어 반환
    public static int GetRaycastLayermaskIdx(string layerMask) => 1 << LayerMask.NameToLayer(layerMask);

    // 전 방향 레이 테스트
    public static void TestRayAllDir(Transform rayTarget)
    {
        Debug.DrawLine(rayTarget.localPosition, leftUpDiagonalDir,    Color.red); // -1, 1 (상단 왼쪽)
        Debug.DrawLine(rayTarget.localPosition, rightUpDiagonalDir,   Color.red); // 1 , 1 (상단 오른쪽)
        Debug.DrawLine(rayTarget.localPosition, leftDownDiagonalDir,  Color.red); // -1 , -1 (하단 왼쪽)
        Debug.DrawLine(rayTarget.localPosition, rightDownDiagonalDir, Color.red); // 1 , -1 (하단 오른쪽)
        Debug.DrawLine(rayTarget.localPosition, Vector2.up,           Color.red); // 윗 방향
        Debug.DrawLine(rayTarget.localPosition, Vector2.left,         Color.red); // 왼쪽 방향
        Debug.DrawLine(rayTarget.localPosition, Vector2.right,        Color.red); // 오른쪽 방향
        Debug.DrawLine(rayTarget.localPosition, Vector2.down,         Color.red); // 아래 방향
    }

    // 전 방향 레이캐스트 지정
    public static void GetRaycastAllDir(out Vector2[] arrRayPos)
    {
         // 여덟 방향 레이캐스트
         //  \ | /       0 1 2
         // --   --      3   4
         //  / | \       5 6 7
        arrRayPos    = new Vector2[8];
        arrRayPos[0] = leftUpDiagonalDir;
        arrRayPos[1] = Vector2.up;
        arrRayPos[2] = rightUpDiagonalDir;
        arrRayPos[3] = Vector2.left;
        arrRayPos[4] = Vector2.right;
        arrRayPos[5] = leftDownDiagonalDir;
        arrRayPos[6] = Vector2.down;
        arrRayPos[7] = rightDownDiagonalDir;
    }

    public static bool CollisionContainsName(GameObject obj, string name) => obj.name.Contains(name);

    public static bool IsSameLayer(GameObject obj, string layerName) => obj.layer == LayerMask.NameToLayer(layerName);


    #endregion
}