<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\CatalagoObjetoRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;

class CatalagoObjetoCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    public function setup()
    {
        CRUD::setModel(\App\Models\CatalagoObjeto::class);
        CRUD::setRoute(config('backpack.base.route_prefix') . '/catalago-objeto');
        CRUD::setEntityNameStrings('catalago objeto', 'catalago objetos');
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'titulo',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'price',
            'label' => 'Precio',
            'type' => 'number'
        ]);
        $this->crud->addColumn([
            'name' => 'swf_name',
            'label' => 'SWF Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'visible',
            'label' => 'Visible',
            'type' => 'check'
        ]);
        $this->crud->addColumn([
            'name' => 'active',
            'label' => 'Activo',
            'type' => 'check'
        ]);


    }

    protected function basicFields()
    {
        $this->crud->addFields([
            [
                'name' => 'titulo',
                'label' => 'Titulo',
                'type' => 'text',
            ],
            [
                'name' => 'descripcion',
                'label' => 'Descripcion',
                'type' => 'text',
            ],
            [
                'name' => 'price',
                'label' => 'Precio',
                'type' => 'number',
                'default' => 0
            ],
            [
                'name' => 'swf_name',
                'label' => 'SWF Nombre',
                'type' => 'text',
            ],
            [
                'name' => 'colores_hex',
                'label' => 'Colores Hexadecimal',
                'type' => 'text',
            ],
            [
                'name' => 'colores_rgb',
                'label' => 'Colores RGB',
                'type' => 'text',
            ],
            [
                'name' => 'espacio_mapabytes',
                'label' => 'Espacion ocupado en MapaBytes',
                'type' => 'text',
            ],
            [
                'name' => 'tipo',
                'label' => 'Tipo objeto',
                'type' => 'select_from_array',
                'options' => config('app.objeto_tipos'),
            ],
            [
                'name' => 'visible',
                'label' => 'Visible',
                'type' => 'checkbox',
            ],
            [
                'name' => 'active',
                'label' => 'Activo',
                'type' => 'checkbox',
            ],
        ]);
    }

    protected function setupCreateOperation()
    {
        CRUD::setValidation(CatalagoObjetoRequest::class);

        $this->basicFields();

    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
