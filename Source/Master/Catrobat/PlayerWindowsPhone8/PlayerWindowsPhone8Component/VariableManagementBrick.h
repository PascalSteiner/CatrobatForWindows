#pragma once
#include "brick.h"
#include "UserVariable.h"
#include "FormulaTree.h"

class VariableManagementBrick :
	public Brick
{
public:
	VariableManagementBrick(TypeOfBrick brickType, string objectReference, FormulaTree *variableFormula, Script *parent);
	virtual void Execute() = 0;

	void SetVariable(UserVariable *variable);

protected:
	UserVariable *m_variable;
	FormulaTree *m_variableFormula;
};