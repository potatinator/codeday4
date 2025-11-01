extends CharacterBody2D

@export var speedBase = 200;
@export var speedSprint = 400;
@export var speedSneak = 100;
@export var HUD: CanvasLayer;
var speed = speedBase;
var vel = Vector2.ZERO;
var score = 0;
var scareReady = false;
var toScare: CharacterBody2D;
var stamina = 1;

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	vel = Vector2.ZERO;
	if Input.is_action_pressed("move_down"):
		vel.y += 1;
	if Input.is_action_pressed("move_up"):
		vel.y -= 1;
	if Input.is_action_pressed("move_left"):
		vel.x -= 1;
	if Input.is_action_pressed("move_right"):
		vel.x += 1;
	if Input.is_action_pressed("sneak"):
		speed = speedSneak;
		if stamina < 1:
			stamina += 0.05*delta;
	else:
		if Input.is_action_pressed("sprint"):
			if stamina > 0:
				speed = speedSprint;
				stamina -= 0.2*delta;
			else:
				speed = 0;
		else:
			speed = speedBase;
			if stamina < 1:
				stamina += 0.025*delta;
		
	if(vel.length() > 0):
		vel = vel.normalized() * speed;
		
	velocity = vel;
	move_and_slide();
	
	HUD.setScore(score);
	HUD.setStamina(stamina);
	if Input.is_action_just_pressed("scare"):
		if scareReady:
			if toScare != null:
				toScare.scare();
		else:
			print("scare unavailible");
	
	pass


func _on_pickup_area_entered(area: Area2D) -> void:
	area.queue_free();
	score += 1;
pass

func _on_scare_area_entered(area: Area2D) -> void:
	if area.get_parent().name.begins_with("child"):
		scareReady = true;
	toScare = area.get_parent();
pass
func _on_scare_area_exited(area: Area2D) -> void:
	if area.get_parent().name.begins_with("child"):
		scareReady = false;
	toScare = null;
	pass
