extends CanvasLayer


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func setScore(score: int):
	$Label.text = "Stolen candy: " + str(score);
pass

func setLives(lives: int):
	$Label2.text = "Bribes: " + str((lives-1));
pass
func setStamina(stamina: float):
	$"Control/stamia bar".scale.x = stamina*225;
	pass
