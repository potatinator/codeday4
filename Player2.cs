using Godot;
using System;

public partial class Player2 : CharacterBody2D
{
	
[Export]
 var speedBase = 200;
[Export]
 var speedSprint = 400;
[Export]
 var speedSneak = 100;
[Export]
 CanvasLayer HUD;
float speed = speedBase;
Vector2 vel = Vector2.ZERO;
int score = 0;
bool scareReady = false;
CharacterBody2D toScare;
var stamina = 1;
var bigChild = false;

# Called when the node enters the scene tree for the first time.
private void _ready() -> void{
	} # Replace with private voidtion body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
private void _process(delta{ float) -> void{
	vel = Vector2.ZERO;
	if (Input.IsActionPressed("move_down"){
		vel.y += 1;
	if (Input.IsActionPressed("move_up"){
		vel.y -= 1;
	if (Input.IsActionPressed("move_left"){
		vel.x -= 1;
	if (Input.IsActionPressed("move_right"){
		vel.x += 1;
	if (Input.IsActionPressed("sneak"){
		speed = speedSneak;
		if (stamina < 1{
			stamina += 0.05*delta;
	else{
		if (Input.IsActionPressed("sprint"){
			if (stamina > 0{
				speed = speedSprint;
				stamina -= 0.2*delta;
			else{
				speed = speedBase;
		else{
			speed = speedBase;
			if (stamina < 1{
				stamina += 0.025*delta;
		
	if(vel.length() > 0){
		vel = vel.normalized() * speed;
		if (vel.y > 0 && vel.x > 0{
			if (vel.y > vel.x{
				$AnimatedSprite2D.play("down");
			if (vel.y < vel.x{
				$AnimatedSprite2D.play("right");
		else{ if (vel.y > 0 && vel.x < 0{
			if (vel.y > abs(vel.x){
				$AnimatedSprite2D.play("down");
			if (vel.y < abs(vel.x){
				$AnimatedSprite2D.play("left");
		else{ if (vel.y < 0 && vel.x > 0{
			if (abs(vel.y) < abs(vel.x){
				$AnimatedSprite2D.play("left");
			if (abs(vel.y) > abs(vel.x){
				$AnimatedSprite2D.play("up");
		else{ if (vel.y < 0 && vel.x < 0{
			if (abs(vel.y) < abs(vel.x){
				$AnimatedSprite2D.play("right");
			if (abs(vel.y) > abs(vel.x){
				$AnimatedSprite2D.play("up");
		else{
			if (vel.y > 0{
				$AnimatedSprite2D.play("down");
			if (vel.y < 0{
				$AnimatedSprite2D.play("up");
			if (vel.x > 0{
				$AnimatedSprite2D.play("right");
			if (vel.x < 0{
				$AnimatedSprite2D.play("left");
	else{
		$AnimatedSprite2D.stop()
		
	velocity = vel;
	move_and_slide();
	
	stamina = clamp(stamina, 0, 1);
	
	HUD.setScore(score);
	HUD.setStamina(stamina);
	
	if (Input.IsActionPressed("scare"){
		if (scareReady{
			if (toScare != null{
				toScare.incrementScare();
	
	}


private void _on_pickup_area_entered(area{ Area2D) -> void{
	area.queue_free();
	score += 1;
	stamina += 0.05;
}

private void _on_scare_area_entered(area{ Area2D) -> void{
	if (area.get_parent().name.begins_with("child"){
		scareReady = true;
		bigChild = false;
	else{ if (area.get_parent().name.begins_with("bigChild"){
		scareReady = true;
		bigChild = true;
		
	toScare = area.get_parent();
}
private void _on_scare_area_exited(area{ Area2D) -> void{
	if (area.get_parent().name.begins_with("child"){
		scareReady = false;
	toScare = null;
	}

}
