#include "Checker.h"

Checker::Checker()
{
}

Checker::Checker(string name)
{
	Checker::playerName = name;
}

Checker::~Checker()
{
}

int Checker::GetX() 
{
	return x;
}

void Checker::SetX(int x)
{
	Checker::x = x;
}

int Checker::GetY()
{
	return y;
}

void Checker::SetY(int y)
{
	Checker::y = y;
}

cv::Scalar Checker::GetHSVmin() {
	return HSVmin;
}
cv::Scalar Checker::GetHSVmax() {
	return HSVmax;
}

void Checker::SetHSVmin(cv::Scalar min) {
	Checker::HSVmin = min;
}
void Checker::SetHSVmax(cv::Scalar max) {
	Checker::HSVmax = max;
}

string Checker::GetPlayerName() {
	return playerName;
}