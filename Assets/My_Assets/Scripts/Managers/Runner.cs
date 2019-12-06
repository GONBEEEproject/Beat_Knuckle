using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Runner : MonoBehaviour
{
    private static Runner instance;
    public static Runner Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Runner>();
            return instance;
        }
    }

    /* 譜面情報系
     * CSVのパスを指定してロードして
     * タイミング情報とレーン情報を保持する
     * Noteクラスを宣言しておく
     */
    [SerializeField]
    private string _csvPath;
    private List<Note> _notes = new List<Note>();

    //スタート前に小音量で流すので切り替える用
    [SerializeField]
    private AudioClip music;

    //レーンごとに飛ばすオブジェクトを変えるので、その分保持
    [SerializeField]
    private GameObject[] _items;

    //フェードアウト用
    [SerializeField]
    private Image _fader;

    //楽曲再生用、一番大事
    private AudioSource _source;

    //プレイしてるかどうかの確認
    private bool _isPlaying;

    //現在どのノーツを生成しているかの保持
    private int _noteID = 0;

    void Start()
    {
        //シーン開始時はフェードインする
        _fader.color = Color.white;
        _fader.DOColor(Color.clear, 3.0f).SetEase(Ease.Linear);

        _source = GetComponent<AudioSource>();
        //譜面データ保持する
        Load();
    }

    void Update()
    {
        //ノーツ探索はプレイ中のみに抑える
        if (_isPlaying)
        {
            SearchNote();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GameStart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameStart()
    {
        StartCoroutine(GameSeq());
    }

    /// <summary>
    /// ゲーム開始時のコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator GameSeq()
    {
        SoundShooter.Instance.GameStart();

        yield return new WaitForSeconds(1.0f);

        //ノーツ生成状態を初期化してプレイを開始する
        _noteID = 0;
        _source.Stop();
        _source.clip = music;
        _source.loop = false;
        _source.volume = 1;
        _source.time = 0;
        _source.Play();
        EffectRunner.Instance.GameStart();
        _isPlaying = true;
    }

    /// <summary>
    /// ノーツの探索ループ
    /// </summary>
    private void SearchNote()
    {
        //ノーツを最後まで生成したらゲーム終了
        if (_noteID < _notes.Count)
        {
            /* ノーツ生成ロジック
             * 保持してるタイミング情報は楽曲再生時間でのジャストなので
             * アニメーションする用のオフセット分を引いて早めに生成する
             * 規模が大きくなった時のために時間は他の場所で管理するのがいい
             */
            if (_notes[_noteID].Timing - TimeController.Instance.Offset < _source.time)
            {
                GenerateNote(_notes[_noteID]);
                _noteID++;
            }
        }
        else
        {
            GameEnd();
            _isPlaying = false;
        }
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    /// <param name="note">該当ノーツクラスを転送してくる</param>
    private void GenerateNote(Note note)
    {
        //生成と同時に初期化
        var g = Instantiate(_items[note.Lane]);
        g.GetComponent<Punch_Item>().Initialize(TimeController.Instance.Offset, note.Lane);
    }

    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    private void GameEnd()
    {
        //無駄にノーツ探索させないためにフラグをたたむ
        _isPlaying = false;
        StartCoroutine(RestartSeq());
    }

    private IEnumerator RestartSeq()
    {
        yield return new WaitForSeconds(_source.clip.length - _source.time - 3.0f);

        _fader.DOColor(Color.white, 3.0f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 譜面ロード
    /// </summary>
    private void Load()
    {
        //譜面データの中身を引っ張ってくる
        var csv = Resources.Load(_csvPath) as TextAsset;
        var reader = new StringReader(csv.text);

        //ロード
        while (reader.Peek() > -1)
        {
            /* 譜面データの中身は1行が1ノーツ
             * 1列目がタイミング情報
             * 2列めがレーン情報
             * ノーツを新規宣言してそれぞれデータを入れたあとに
             * ノーツリストに追加してループを出る
             */
            string row = reader.ReadLine();
            string[] values = row.Split(',');
            var n = new Note();
            n.Timing = float.Parse(values[0]);
            n.Lane = int.Parse(values[1]);

            _notes.Add(n);
        }
    }
}