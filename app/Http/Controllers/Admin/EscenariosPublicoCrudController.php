<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\EscenariosPublicoRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;

class EscenariosPublicoCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    public function setup()
    {
        CRUD::setModel(\App\Models\EscenariosPublico::class);
        CRUD::setRoute(config('backpack.base.route_prefix') . '/escenarios-publico');
        $this->filterList();
        CRUD::setEntityNameStrings('escenarios', 'escenarios');
    }

    private function filterList()
    {
        $this->crud->addClause('orderBy', 'id', 'ASC');
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'nombre',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'Prioridad',
            'label' => 'Prioridad',
            'type' => 'number'
        ]);
        $this->crud->addColumn([
            'name' => 'max_visitantes',
            'label' => 'Maximo personas',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'uppert',
            'label' => 'Precio upper',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'coco',
            'label' => 'Precio coco',
            'type' => 'text'
        ]);
    }

    protected function setupCreateOperation()
    {
        CRUD::setValidation(EscenariosPublicoRequest::class);

        CRUD::setFromDb(); // fields
    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
