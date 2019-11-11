#pragma once
#include <string>
#include <opencv2\cv.hpp>

using namespace std;

class Checker
{
public:
	Checker();
	~Checker();

	Checker(string name);

	int GetX();
	void SetX(int x);
	int GetY();
	void SetY(int y);

	cv::Scalar GetHSVmin();
	cv::Scalar GetHSVmax();

	void SetHSVmin(cv::Scalar min);
	void SetHSVmax(cv::Scalar max);

	string GetPlayerName();


private :
	int x, y;
	string playerName;

	cv::Scalar HSVmin, HSVmax;

};

