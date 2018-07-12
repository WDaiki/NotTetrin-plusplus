﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Utility;
using NotTetrin.Utility.Physics2D;
using NotTetrin.Utility.Tiling;

namespace NotTetrin.Ingame.Marathon {
    [RequireComponent(typeof(Renderer), typeof(TileCreator))]
    public class CollidersGroup : MonoBehaviour {
        [SerializeField] DensityIndicator indicatorPrefab;

        private new Renderer renderer;
        private List<GameObject> minos = new List<GameObject>();
        private ColliderGroup group;
        private Instantiator instantiator;
        private DensityIndicator indicator;

        public bool IsEntered => group.EnteredAll;
        public int EnteredObjectCount => minos.Count();
        private ParticleSystem MinoDeleteEffect;

        private void Awake() {
            renderer = GetComponent<Renderer>();
            MinoDeleteEffect = GetComponentInChildren<ParticleSystem>();
            MinoDeleteEffect.Stop();
        }

        private void Start() {
            var objects = GetComponent<TileCreator>().Create();
            var colliders = objects.Select(o => o.GetComponent<ColliderHelper>());
            group = new ColliderGroup(colliders);
        }

        public void Initialize(Instantiator instantiator, GameObject wall) {
            this.instantiator = instantiator;

            this.indicator = Instantiate(indicatorPrefab, transform, false);
            var rate = gameObject.size().y / indicator.gameObject.size().y;
            var scale = indicator.gameObject.transform.localScale;
            scale.y *= rate;
            indicator.gameObject.transform.localScale = scale;
            indicator.Initialize(wall);
        }

        private void Update() {
            var density = (float)group.EnterCount / group.Children.Count();
            indicator.UpdateDensity(density);
            renderer.enabled = IsEntered;
        }

        public void DeleteMino() {
            Debug.Log("ミノ削除");
            MinoDeleteEffect.Play();
            foreach (var mino in minos) {
                Debug.Log(mino + "削除");
                mino.transform.Translate(10000, 10000, 10000);  // OnTriggerExit2Dを呼び出すため範囲外へ移動
                instantiator.Destroy(mino, 1.0f);    // 即消しだと判定が残るから時間差攻撃
            }
            minos.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            minos.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            minos.Remove(collision.gameObject);
        }
    }
}