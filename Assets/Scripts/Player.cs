﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    const KeyCode MOVE_LEFT_KEY = KeyCode.A;
    const KeyCode MOVE_DOWN_KEY = KeyCode.S;
    const KeyCode MOVE_RIGHT_KEY = KeyCode.D;
    const KeyCode JUMP_KEY = KeyCode.Space;
    const KeyCode kMicrosoftAButton = KeyCode.Joystick1Button0;

    public Rigidbody2D rigid;
    public bool inAir;
    public float move_speed = 4.0f;
    public float jump_scale_factor = 3.0f;
    public int player;
    public bool useController;
    public bool has_triggered = false;
    public bool right_trigger_down = false;
    public bool jump_reset;

    float jump_speed;

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        inAir = false;
        jump_speed = rigid.gravityScale * jump_scale_factor;
        jump_reset = false;
    }

    void Update() {
        if (this.GetComponent<PlayerManager>().death == true)
            return;
        if (Input.GetKeyDown(KeyCode.Q)) {
            useController = true;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            useController = false;
        }
        updateMovement();
        updateJump();
    }

    void updateMovement() {
        float x_direction = 0f;
        if (useController) {
            x_direction = Input.GetAxis(Controls.axes_codes[player, Controls.axis_left_joy_hor]);
            if (Mathf.Abs(x_direction) < 0.5f) {
                x_direction = 0.0f;
            }
        }
        else {
            bool a = Input.GetKey(MOVE_LEFT_KEY);
            bool d = Input.GetKey(MOVE_RIGHT_KEY);
            x_direction = a ? -1 : (d ? 1 : 0);
        }
        rigid.velocity = new Vector2(x_direction * move_speed, rigid.velocity.y);
    }

    void updateJump() {
		if ((Input.GetAxis(Controls.axes_codes[player, Controls.axis_right_trigger])) < 0.1f) {
			print ("reset jump");
			jump_reset = true;
		}
		if ((Input.GetAxis(Controls.axes_codes[player, Controls.axis_right_trigger])) > 0.1f) {
			print ("right trigger down");
			right_trigger_down = true;
		}

		if ((useController && jump_reset && right_trigger_down) && (!inAir)) {
            jump();
            has_triggered = true;
            jump_reset = false;
        }
        else if (Input.GetKeyDown(JUMP_KEY) && (!inAir)) {
            jump();
        }
        right_trigger_down = false;
    }

    void jump() {
        //transform.Translate(new Vector3 (0, 1));
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + jump_speed);
        inAir = true;
        print("jump!");
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "LevelTerrain") {
            inAir = false;
        }
    }
}
