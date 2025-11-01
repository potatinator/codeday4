extends Area2D

@export var candy: PackedScene;
@export var r = 25;
@export var speed = 2;
@export var variation = 100;
var scareable = false;
var seen = false;
var inRange = false;
var localSpeed = Vector2(0,0);
var globalSpeed = Vector2(0,0);
var rotSpeed = 0;
var stopCounter = 0;
var startCounter = 0;

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	scareable = !seen && inRange;
	stopCounter += randf();
	
	localSpeed.y += randf()*variation*delta;
	localSpeed.y -= randf()*variation*delta*0.1;
	localSpeed.x += randf()*variation*delta*0.1;
	localSpeed.x -= randf()*variation*delta*0.1;
	rotSpeed += (randf()-0.5)*delta;
	rotSpeed = clamp(rotSpeed, -0.02, 0.02);
	localSpeed = clamp(localSpeed.length(), -speed, speed)*localSpeed.normalized();
	globalSpeed = localSpeed.rotated(rotation);
	rotation += rotSpeed;
	
	if stopCounter <= 50:
		position += globalSpeed;
		startCounter = 0;
	else:
		startCounter += randf();
		if startCounter >= 20:
			stopCounter = 0;
	
	$Label.visible = scareable;
	
	pass

func scare():
	if scareable:
		for i in 5:
			var a = candy.instantiate();
			a.position = position + Vector2(randi_range(-r, r), randi_range(-r, r))
			get_parent().add_child(a);
		queue_free();
pass

func _on_area_2d_area_entered(area: Area2D) -> void:
	seen = true;
pass
func _on_area_2d_area_exited(area: Area2D) -> void:
	seen = false;
pass

func _on_area_entered(area: Area2D) -> void:
	inRange = true;
	pass # Replace with function body.
func _on_area_exited(area: Area2D) -> void:
	inRange = false;
	pass # Replace with function body.
