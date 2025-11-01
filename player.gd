extends Area2D

@export var speedBase = 200;
@export var speedSprint = 400;
@export var speedSneak = 100;
var speed = speedBase;
var vel = Vector2.ZERO;

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
	else:
		if Input.is_action_pressed("sprint"):
			speed = speedSprint;
		else:
			speed = speedBase;
		
	if(vel.length() > 0):
		vel = vel.normalized() * speed * delta;
		
	position += vel;
	pass
