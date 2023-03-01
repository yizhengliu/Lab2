using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Hazard : MonoBehaviour
{
    Vector2 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    public AnimationCurve curve;

    // Update is called once per frame
    void Update()
    {
        transform.position = 
            new Vector2(originalPos.x, curve.Evaluate(Time.time) + originalPos.y);   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
