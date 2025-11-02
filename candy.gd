extends Area2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var col =  hsv_to_rgb(randf(), 1, 1).lightened(10);
	$ColorRect.color = col;
	$PointLight2D.color = col;
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func hsv_to_rgb(h, s, v, a = 1):
	#based on code at
	#http://stackoverflow.com/questions/51203917/math-behind-hsv-to-rgb-conversion-of-colors
	var r
	var g
	var b

	var i = floor(h * 6)
	var f = h * 6 - i
	var p = v * (1 - s)
	var q = v * (1 - f * s)
	var t = v * (1 - (1 - f) * s)

	match (int(i) % 6):
		0:
			r = v
			g = t
			b = p
		1:
			r = q
			g = v
			b = p
		2:
			r = p
			g = v
			b = t
		3:
			r = p
			g = q
			b = v
		4:
			r = t
			g = p
			b = v
		5:
			r = v
			g = p
			b = q
	return Color(r, g, b, a)
