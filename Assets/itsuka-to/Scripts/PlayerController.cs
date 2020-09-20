using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 参考: http://negi-lab.blog.jp/MouseFollow2D
/// </summary>
public class PlayerController : MonoBehaviour, IBulletDamaged
{
    private Vector3 targetPos;
    private Vector3 screenToWorldPointPosition;

    [SerializeField]
    private float hidePosY = -10f;  // シューティング以外の画面外に隠れる時

    public int maxHp = 100;

    public ReactiveProperty<int> hp = new ReactiveProperty<int>(100);

    // X, Y座標の移動可能範囲
    [System.Serializable]
    public class Bounds
    {
        public float xMin, xMax, yMin, yMax;
    }
    [SerializeField] Bounds bounds;

    // 補間の強さ（0f～1f） 。0なら追従しない。1なら遅れなしに追従する。
    [SerializeField, Range(0f, 1f)] private float followStrength;

    private void Awake()
    {

    }

    void Start()
    {
        hp.Value = maxHp;  // とりあえず初期HPはマックスで初期化
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.EGamePhase phase = GameManager.Instance.Phase.Value;

        if(phase == GameManager.EGamePhase.SHOOTING_PHASE)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            targetPos = transform.position;
            targetPos.y = hidePosY;  //隠す
        }

        // X, Y座標の範囲を制限する
        targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

        // Z座標を修正する
        targetPos.z = 0f;

        // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
        transform.position = Vector3.Lerp(transform.position, targetPos, followStrength);

    }

    public void AddDamage(int damage)
    {
        int hp_t = hp.Value;
        hp_t -= damage;
        if(hp_t < 0)
        {
            hp.Value = 0;
        }
        else
        {
            hp.Value = hp_t;
        }
        
    }
}
