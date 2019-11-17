
#include <sstream>
#include <string>
#include <iostream>
#include <opencv2\highgui.hpp>
#include <vector>
#include "C:/opencv/build/include/opencv2/imgproc.hpp"
#include "Checker.h"

using namespace cv;
using namespace std;

//initial min and max HSV filter values.
//these will be changed using trackbars
int H_MIN = 0;
int H_MAX = 256;
int S_MIN = 0;
int S_MAX = 256;
int V_MIN = 0;
int V_MAX = 256;
//default capture width and height
const int FRAME_WIDTH = 1280;
const int FRAME_HEIGHT = 720;
//max number of objects to be detected in frame
const int MAX_NUM_OBJECTS = 50;
//minimum and maximum object area
const int MIN_OBJECT_AREA = 20 * 20;
const int MAX_OBJECT_AREA = FRAME_HEIGHT * FRAME_WIDTH / 1.5;
//names that will appear at the top of each window
const string windowName = "Original Image";
const string windowName1 = "HSV Image";
const string windowName2 = "Thresholded Image";
const string windowName3 = "After Morphological Operations";
const string trackbarWindowName = "Trackbars";
bool p1Bigger = false;

struct Tile {
	Point start;
	Point end;
	string name;
	int number;
	Point middle;
};

Point edgePoints[18];
vector <Tile> tiles;
vector <Checker> player1_checkers;
vector <Checker> player2_checkers;

void on_trackbar(int, void*)
{//This function gets called whenever a
	// trackbar position is changed
}

string intToString(int number) {

	std::stringstream ss;
	ss << number;
	return ss.str();
}

void createTrackbars() {
	//create window for trackbars
	namedWindow(trackbarWindowName, 0);
	//create memory to store trackbar name on window
	char TrackbarName[50];
	sprintf_s(TrackbarName, "H_MIN", H_MIN);
	sprintf_s(TrackbarName, "H_MAX", H_MAX);
	sprintf_s(TrackbarName, "S_MIN", S_MIN);
	sprintf_s(TrackbarName, "S_MAX", S_MAX);
	sprintf_s(TrackbarName, "V_MIN", V_MIN);
	sprintf_s(TrackbarName, "V_MAX", V_MAX);
	//create trackbars and insert them into window
	//3 parameters are: the address of the variable that is changing when the trackbar is moved(eg.H_LOW),
	//the max value the trackbar can move (eg. H_HIGH), 
	//and the function that is called whenever the trackbar is moved(eg. on_trackbar)
	//                                  ---->    ---->     ---->      
	createTrackbar("H_MIN", trackbarWindowName, &H_MIN, H_MAX, on_trackbar);
	createTrackbar("H_MAX", trackbarWindowName, &H_MAX, H_MAX, on_trackbar);
	createTrackbar("S_MIN", trackbarWindowName, &S_MIN, S_MAX, on_trackbar);
	createTrackbar("S_MAX", trackbarWindowName, &S_MAX, S_MAX, on_trackbar);
	createTrackbar("V_MIN", trackbarWindowName, &V_MIN, V_MAX, on_trackbar);
	createTrackbar("V_MAX", trackbarWindowName, &V_MAX, V_MAX, on_trackbar);
}

void drawObject(vector <Checker> checkers, Mat& frame) {
	for (int i = 0; i < checkers.size(); i++)
	{
		circle(frame, Point(checkers.at(i).GetX() , checkers.at(i).GetY()), 10, Scalar(0, 0, 255));
		putText(frame, intToString(checkers.at(i).GetX()) + " , " + intToString(checkers.at(i).GetY()), cv::Point(checkers.at(i).GetX(), checkers.at(i).GetY() + 20), 1, 1, Scalar(0, 255, 0));
		putText(frame, checkers.at(i).GetPlayerName(), cv::Point(checkers.at(i).GetX(), checkers.at(i).GetY() + 40), 1, 1, Scalar(255, 255, 0));
	}
}
void morphOps(Mat & thresh) {
	//create structuring element that will be used to "dilate" and "erode" image.
	//the element chosen here is a 3px by 3px rectangle
	Mat erodeElement = getStructuringElement(MORPH_RECT, Size(3, 3));
	//dilate with larger element so make sure object is nicely visible
	Mat dilateElement = getStructuringElement(MORPH_RECT, Size(8, 8));

	erode(thresh, thresh, erodeElement);
	erode(thresh, thresh, erodeElement);

	dilate(thresh, thresh, dilateElement);
	dilate(thresh, thresh, dilateElement);
}


void CalcualteBoardEdges(Point P1, Point P2, Mat& frame) {
	
	int boardLenght = 0;
	int boardHeight = 0;

	Point P0;

	if (P2.x > P1.x)
	{
		p1Bigger = false;
		boardLenght = P2.x - P1.x;
		boardHeight = P2.y - P1.y;
		 P0 = P1;
	}
	else
	{
		p1Bigger = true;
		boardLenght = P1.x - P2.x;
		boardHeight = P1.y - P2.y;
		P0 = P2;
	}

	Point P11 = Point (boardLenght / 8 + P0.x,P0.y);
	Point P22 = Point(boardLenght / 8 * 2 + P0.x, P0.y);
	Point P3 = Point(boardLenght / 8 * 3 + P0.x, P0.y);
	Point P4 = Point(boardLenght / 8 * 4 + P0.x, P0.y);
	Point P5 = Point(boardLenght / 8 * 5 + P0.x, P0.y);
	Point P6 = Point(boardLenght / 8 * 6 + P0.x, P0.y);
	Point P7 = Point(boardLenght / 8 * 7 + P0.x, P0.y);
	Point P8 = Point(boardLenght + P0.x, P0.y);

	Point P32 = P0;
	Point P31 = Point(P0.x, boardHeight / 8 +P0.y);
	Point P30 = Point(P0.x, boardHeight / 8 * 2 + P0.y);
	Point P29 = Point(P0.x, boardHeight / 8 * 3 + P0.y);
	Point P28 = Point(P0.x, boardHeight / 8 * 4 + P0.y);
	Point P27 = Point(P0.x, boardHeight / 8 * 5 + P0.y);
	Point P26 = Point(P0.x, boardHeight / 8 * 6 + P0.y);
	Point P25 = Point(P0.x, boardHeight / 8 * 7 + P0.y);
	Point P24 = Point(P0.x, boardHeight + P0.y);

	edgePoints[0] = P0;
	edgePoints[1] = P11;
	edgePoints[2] = P22;
	edgePoints[3] = P3;
	edgePoints[4] = P4;
	edgePoints[5] = P5;
	edgePoints[6] = P6;
	edgePoints[7] = P7;
	edgePoints[8] = P8;
	edgePoints[9] = P24;
	edgePoints[10] = P25;
	edgePoints[11] = P26;
	edgePoints[12] = P27;
	edgePoints[13] = P28;
	edgePoints[14] = P29;
	edgePoints[15] = P30;
	edgePoints[16] = P31;
	edgePoints[17] = P32;

}

void DrawEdges(Point boardEdge[18], Mat& frame) {
	for (int i = 0; i < 18; i++)
	{
	circle(frame, Point(boardEdge[i].x, boardEdge[i].y), 10, Scalar(0, 0, 255));
	putText(frame, intToString(i), boardEdge[i], 1, 1, Scalar(255, 255, 0));
	}
}

void CalculateTiles(Point boardEdge[18], Mat& frame) {
	string number = "ABCDEFGH";
	string letter = "12345678";
	int tileNum = 0;
	tiles.clear();

	for (int i=0;i<8;i++)
		for (int j=0; j < 8; j++)
		{
			Tile tile;
			tile.number = tileNum;
			string tn = "  ";
			tn[0] = number[i];
			tn[1] = letter[j];
			tile.name = tn;
			tile.start = Point(boardEdge[j].x,boardEdge[17-i].y);
			tile.end = Point(boardEdge[j+1].x, boardEdge[16-i].y);
			tile.middle = Point((tile.end.x - tile.start.x) / 2 + tile.start.x, (tile.end.y - tile.start.y) / 2 + tile.start.y);
			tiles.push_back(tile);
			tileNum++;
		}
}

void GiveCheckersTiles(Checker &checker, Mat& frame)
{
	if (p1Bigger)
	for (int i = 0; i < 64; i++) {
		if (checker.GetX() <= tiles[i].end.x 
			&& checker.GetX() >= tiles[i].start.x
			&& checker.GetY() >= tiles[i].start.y
			&& checker.GetY() <= tiles[i].end.y)
					{
						checker.tileName[0] = tiles[i].name[0];
						checker.tileName[1] = tiles[i].name[1];
						checker.tileNumber = tiles[i].number;
					}
	}
	else
		for (int i = 0; i < 64; i++) {
			if (checker.GetX() <= tiles[i].end.x
				&& checker.GetX() >= tiles[i].start.x
				&& checker.GetY() <= tiles[i].start.y
				&& checker.GetY() >= tiles[i].end.y)
			{
				checker.tileName[0] = tiles[i].name[0];
				checker.tileName[1] = tiles[i].name[1];
				checker.tileNumber = tiles[i].number;
			}
		}
}

void DrawTiles(Mat& frame)
{
	for (int i = 0; i < 64; i++) {
		if (i % 2 == 0)
		{
		rectangle(frame, tiles[i].start, tiles[i].end, Scalar(0, 0, 255), 8, 1);
		putText(frame, tiles.at(i).name, tiles[i].middle, 1, 1, Scalar(0, 0,255));
		}
		else
		{
		rectangle(frame, tiles[i].start, tiles[i].end, Scalar(0, 255, 0), 8, 1);
		putText(frame,tiles.at(i).name, tiles[i].middle, 1, 1, Scalar(0, 0,255));
		}
	}
}

void trackFilteredObject(Mat threshold, Mat HSV, Mat & cameraFeed,string name) {

	vector <Checker> borders;

	Mat temp;
	threshold.copyTo(temp);
	//these two vectors needed for output of findContours
	vector< vector<Point> > contours;
	vector<Vec4i> hierarchy;
	//find contours of filtered image using openCV findContours function
	findContours(temp, contours, hierarchy, RETR_CCOMP, CHAIN_APPROX_SIMPLE);
	//use moments method to find our filtered object
	double refArea = 0;
	bool objectFound = false;
	if (hierarchy.size() > 0) {
		int numObjects = hierarchy.size();
		//if number of objects greater than MAX_NUM_OBJECTS we have a noisy filter
		if (numObjects < MAX_NUM_OBJECTS) {
			for (int index = 0; index >= 0; index = hierarchy[index][0]) {

				Moments moment = moments((cv::Mat)contours[index]);
				double area = moment.m00;

				//if the area is less than 20 px by 20px then it is probably just noise
				//if the area is the same as the 3/2 of the image size, probably just a bad filter
				//we only want the object with the largest area so we safe a reference area each
				//iteration and compare it to the area in the next iteration.
				if (area > MIN_OBJECT_AREA) {

					Checker checker(name);

					checker.SetX(moment.m10 / area);
					checker.SetY(moment.m01 / area);

					if (checker.GetPlayerName() == "Mr.White")
					{
						player1_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "Mr.Black")
					{
						player2_checkers.push_back(checker);
					}

					if (checker.GetPlayerName() == "border")
					{
						borders.push_back(checker);
					}

					objectFound = true;

				}
				else objectFound = false;


			}
			//let user know you found an object
			if (objectFound == true) {
				drawObject(borders, cameraFeed);
				if (borders.size() == 2)
				{
					CalcualteBoardEdges(Point (borders[0].GetX(), borders[0].GetY()), Point(borders[1].GetX(), borders[1].GetY()), cameraFeed);
					DrawEdges(edgePoints, cameraFeed);
					CalculateTiles(edgePoints, cameraFeed);
					DrawTiles(cameraFeed);
				}

				if (tiles.size() == 64)
				for (int i = 0; i < player1_checkers.size(); i++)
				{
					circle(cameraFeed, Point(player1_checkers.at(i).GetX(), player1_checkers.at(i).GetY()), 30, Scalar(0, 0, 0));
					GiveCheckersTiles(player1_checkers[i], cameraFeed);
					putText(cameraFeed, player1_checkers[i].tileName, cv::Point(player1_checkers.at(i).GetX(), player1_checkers.at(i).GetY() + 20), 1, 1, Scalar(0, 255, 0));
				}

				if (tiles.size() == 64)
				for (int i = 0; i < player2_checkers.size(); i++)
				{
					circle(cameraFeed, Point(player2_checkers.at(i).GetX(), player2_checkers.at(i).GetY()), 30, Scalar(255, 255, 255));
					GiveCheckersTiles(player2_checkers[i], cameraFeed);
					putText(cameraFeed, player2_checkers[i].tileName, cv::Point(player2_checkers.at(i).GetX(), player2_checkers.at(i).GetY() + 20), 1, 1, Scalar(0, 0, 255));
				}


			}

		}
		else putText(cameraFeed, "TOO MUCH NOISE! ADJUST FILTER", Point(0, 50), 1, 2, Scalar(0, 0, 255), 2);
	}
}


void CameraIdDetection() {
	Mat cameraFeed;
	Mat threshold;
	Mat HSV;
	for (int i = 0; i < 999; i++) {

		try {
			VideoCapture capture;
			capture.open(i);
			capture.set(CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
			capture.set(CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
			capture.read(cameraFeed);
			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			cout << i << ": this number is ID of working camera" << endl;
		}
		catch (...)
		{
		}
	}
}

void DrawCheckers() {

	char board[64];
	for (int i = 0; i < 64; i++)
	{
		board[i] = 'O';
	}
	for (int i = 0; i < player1_checkers.size(); i++)
		if (player1_checkers[i].tileNumber != 0) board[player1_checkers[i].tileNumber] = 'W';

	for (int i = 0; i < player2_checkers.size(); i++)
		if (player2_checkers[i].tileNumber != 0) board[player2_checkers[i].tileNumber] = 'B';

	for (int i = 0; i < 8; i++)
	{
	for (int j = 0; j < 8; j++)
	{
		cout << board[j * i + j];
	}
	cout << endl;
	}
	cout << "----------------" << endl;
}


int main(int argc, char* argv[])
{
	//use this if if you need to detect your camera
	//CameraIdDetection();

	//if we would like to calibrate our filter values, set to true.
	bool calibrationMode = false;

	//Matrix to store each frame of the webcam feed
	Mat cameraFeed;
	Mat threshold;
	Mat HSV;

	if (calibrationMode) {
		//create slider bars for HSV filtering
		createTrackbars();
	}
	//video capture object to acquire webcam feed
	VideoCapture capture;
	//open capture object at location zero (default location for webcam)
	capture.open(701);
	//set height and width of capture frame
	capture.set(CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
	capture.set(CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
	//start an infinite loop where webcam feed is copied to cameraFeed matrix
	//all of our operations will be performed within this loop


	while (1) {
		//store image to matrix
		capture.read(cameraFeed);
		//convert frame from BGR to HSV colorspace
		cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);

		if (calibrationMode == true) {
			//if in calibration mode, we track objects based on the HSV slider values.
			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			inRange(HSV, Scalar(H_MIN, S_MIN, V_MIN), Scalar(H_MAX, S_MAX, V_MAX), threshold);
			morphOps(threshold);
			imshow(windowName2, threshold);
			trackFilteredObject(threshold, HSV, cameraFeed,"");
		}
		else {

			Checker player1("Mr.White"), player2("Mr.Black"), border("border");

			player1.SetHSVmin(Scalar(0,0,255));
			player1.SetHSVmax(Scalar(0,0, 256));

			player2.SetHSVmin(Scalar(0, 0, 0));
			player2.SetHSVmax(Scalar(256, 256, 30));

			border.SetHSVmin(Scalar(100, 255, 190));
			border.SetHSVmax(Scalar(110, 256, 256));

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, player1.GetHSVmin(), player1.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed,"Mr.White");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, player2.GetHSVmin(), player2.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "Mr.Black");

			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//medianBlur(HSV, HSV, 12);
			inRange(HSV, border.GetHSVmin(), border.GetHSVmax(), threshold);
			morphOps(threshold);
			trackFilteredObject(threshold, HSV, cameraFeed, "border");

			DrawCheckers();
			player1_checkers.clear();
			player2_checkers.clear();
		}

		imshow(windowName, cameraFeed);
		waitKey(30); // 30 ms delay
	}


	return 0;
}
